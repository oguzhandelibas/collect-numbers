using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CollectNumbers
{
    public class GridManager : AbstractSingleton<GridManager>
    {
        [SerializeField] private NumberBehaviour numberBehaviourPrefab;
        [SerializeField] private Transform gridParent;
        private float _gridOffset;
        public NumberBehaviour[,] _gridElements;
        public Vector2[,] _gridElementPositions;
        private LevelData _levelData;

        private void Initialize(LevelData levelData)
        {
            _levelData = levelData;
            CreateGrid(levelData.gridSize, levelData.Elements);
        }
        
        public void SetPositions(List<NumberBehaviour> matchedElements, List<NumberBehaviour> fallingElements)
        {
            float growDuration = 0.2f; // Büyüme süresi
            float shrinkDuration = 0.2f; // Küçülme süresi
            float targetScaleFactor = 1.2f; ;
            
            Sequence sequence1 = DOTween.Sequence();
            foreach (var element in matchedElements)
            {
                SelectedColor selectedColor = ElementGenerator.GetSelectedColor(element.selectedNumber);

                GoalManager.Instance.DecreaseGoalCount(selectedColor, element.gameObject);
                element.ResetClickCount();
                element.isActive = false;

                Vector3 originalScale = new Vector3(1,1,1);
                Vector3 targetScale = originalScale * targetScaleFactor;

                Sequence mySequence = DOTween.Sequence(); // Her eleman için yeni bir Sequence oluşturun
                mySequence.Append(element.transform.GetChild(1).DOScale(targetScale, growDuration)
                        .SetEase(Ease.InOutQuad).OnComplete(() => element.Explode(true))) // Büyütme
                    .Append(element.transform.GetChild(1).DOScale(Vector3.zero, shrinkDuration)
                        .SetEase(Ease.OutQuad)); // Küçültme
                
                sequence1.Join(mySequence);
            }

            
            sequence1.OnComplete((() =>
            {
                foreach (var gridElement in fallingElements)
                {
                    RectTransform rectTransform = gridElement.GetComponent<RectTransform>();
                    rectTransform.DOAnchorPos(_gridElementPositions[gridElement.index.x, gridElement.index.y], 0.2f).SetEase(Ease.InOutQuad);
                }
    
                foreach (var t in matchedElements)
                {
                    NumberBehaviour element = t;
                
                    element = ElementGenerator.GenerateRandomElement(element, _levelData.movementRight);
                    element.transform.GetChild(1).gameObject.SetActive(false);
                    element.transform.position += Vector3.up * 5;
                    RectTransform rectTransform = t.GetComponent<RectTransform>();
                    element.Explode(false);
                    MMVibrationManager.Haptic(HapticTypes.MediumImpact, false, true, this);
                
                    element.transform.GetChild(1).localScale = new Vector3(1,1,1);
                    element.transform.GetChild(1).gameObject.SetActive(true);
                
                    rectTransform.DOAnchorPos(_gridElementPositions[element.index.x, element.index.y], 0.2f).SetEase(Ease.InOutQuad);
                }
                
                List<NumberBehaviour> list = _gridElements.Cast<NumberBehaviour>().ToList();
                list = list.OrderBy(nb => nb.index.x).ThenBy(nb => nb.index.y).ToList();

                int cols = _gridElements.GetLength(1);
                for (int i = 0; i < list.Count; i++)
                {
                    int row = i / cols;
                    int col = i % cols;
                    _gridElements[row, col] = list[i];
                }
            
                MatchController.FindAllMatches(_gridElements, _levelData.movementRight, false);
            }));
            
            
            
        }
        
        //grid elementlerinin pozisyonunu indexlere göre ayarla

        private void CreateGrid(Vector2Int gridSize, Element[] elements)
        {
            Debug.Log($"Grid Size: {gridSize}");

            int rowCount = gridSize.x;
            int columnCount = gridSize.y;
            Element[,] elements2D = ConvertTo2DArray(elements, rowCount, columnCount);
            _gridElements = new NumberBehaviour[gridSize.x, gridSize.y];
            _gridElementPositions = new Vector2[gridSize.x, gridSize.y];

            #region Cell Scale

            RectTransform parentRect = gridParent.GetComponent<RectTransform>();
            var rect = parentRect.rect;
            float cellScaleX = rect.width / gridSize.x / 1.5f;
            float cellScaleY = rect.height / gridSize.y / 1.5f;
            float cellScale = Mathf.Min(cellScaleX, cellScaleY);

            #endregion

            float offsetX = (parentRect.rect.width - gridSize.x * cellScale) / 2.0f;

            for (int x = 0; x < rowCount; x++)
            {
                for (int y = 0; y < columnCount; y++)
                {
                    float posX = (x * cellScale) + cellScale / 2f - parentRect.rect.width / 2f + offsetX;
                    float posY = -(y * cellScale);

                    NumberBehaviour numberBehaviour = Instantiate(numberBehaviourPrefab, gridParent);
                    RectTransform cellRect = numberBehaviour.GetComponent<RectTransform>();
                    cellRect.sizeDelta = new Vector2(cellScale, cellScale);

                    Vector2 cellPositon = new Vector2(posX, posY);
                    cellRect.anchoredPosition = cellPositon;
                    _gridElements[x, y] = numberBehaviour;
                    _gridElementPositions[x, y] = cellPositon;
                    numberBehaviour.index = new Vector2Int(x, y);

                    numberBehaviour.selectedNumber = elements2D[x, y].selectedNumber;

                    if (numberBehaviour.selectedNumber != SelectedNumber.Null)
                    {
                        numberBehaviour.Initialize(
                            ElementGenerator.GetRandomElementContext(elements2D[x, y].selectedNumber),
                            ElementGenerator.GetColor(elements2D[x, y].selectedNumber), elements2D[x, y].selectedNumber,
                            _levelData.movementRight);
                        numberBehaviour.isHolded = true;
                    }
                    else
                    {
                        numberBehaviour =
                            ElementGenerator.GenerateRandomElement(numberBehaviour, _levelData.movementRight);
                        //matchController.CheckSpecialMatch(_gridElements, gridSize, x, y);
                    }
                }
            }

            MatchController.FindAllMatches(_gridElements, _levelData.movementRight, true);
        }

        #region Helper Methods

        private Element[,] ConvertTo2DArray(Element[] elements, int rowCount, int columnCount)
        {
            Element[,] elements2d = new Element[rowCount, columnCount];
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    elements2d[i, j] = elements[j * rowCount + i];
                }
            }

            return elements2d;
        }

        #endregion


        private void ChangeGridElement(NumberBehaviour numberBehaviour)
        {
            SO_Manager.Load_SO<LevelSignals>().OnDecreaseMoveCount?.Invoke();
            ElementGenerator.GenerateRandomElement(numberBehaviour, _levelData.movementRight);
            MatchController.FindAllMatches(_gridElements, _levelData.movementRight, false);
        }

        #region EVENT SUBSCRIPTION

        private void OnEnable()
        {
            SO_Manager.Load_SO<GridSignals>().OnGridElementChanged += ChangeGridElement;
            LevelSignals levelSignals = SO_Manager.Load_SO<LevelSignals>();
            levelSignals.OnLevelInitialize += Initialize;
        }

        private void OnDisable()
        {
            SO_Manager.Load_SO<GridSignals>().OnGridElementChanged -= ChangeGridElement;
            LevelSignals levelSignals = SO_Manager.Load_SO<LevelSignals>();
            levelSignals.OnLevelInitialize -= Initialize;
        }

        #endregion
    }
}