using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CollectNumbers
{
    public class GridManager : AbstractSingleton<GridManager>
    {
        [SerializeField] private NumberBehaviour numberBehaviourPrefab;
        [SerializeField] private Transform gridParent;
        private float _gridOffset;
        public NumberBehaviour[] _gridElements;
        public Vector2[] _gridElementPositions;
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
            float targetScaleFactor = 1.2f;

            Sequence completeSequence = DOTween.Sequence();
            foreach (var element in matchedElements)
            {
                SelectedColor selectedColor = SelectedColor.Null;
                switch (element.selectedNumber)
                {
                    case SelectedNumber.One:
                        selectedColor = SelectedColor.Red;
                        break;
                    case SelectedNumber.Two:
                        selectedColor = SelectedColor.Green;
                        break;
                    case SelectedNumber.Three:
                        selectedColor = SelectedColor.Blue;
                        break;
                    case SelectedNumber.Four:
                        selectedColor = SelectedColor.Orange;
                        break;
                    case SelectedNumber.Five:
                        selectedColor = SelectedColor.Purple;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                GoalManager.Instance.DecreaseGoalCount(selectedColor);
                
                element.isActive = false;
                Vector3 originalScale = element.transform.localScale;
                Vector3 targetScale = originalScale * targetScaleFactor;

                Sequence mySequence = DOTween.Sequence(); // Her eleman için yeni bir Sequence oluşturun
                mySequence
                    .Append(element.transform.DOScale(targetScale, growDuration).SetEase(Ease.InOutQuad)) // Büyütme
                    .Append(element.transform.DOScale(Vector3.zero, shrinkDuration).SetEase(Ease.OutQuad)) // Küçültme
                    .OnComplete(() =>
                    {
                        element.gameObject.SetActive(false);
                        element.transform.localScale = originalScale;
                    }); // Küçültme tamamlandığında objeyi devre dışı bırak

                completeSequence.Join(mySequence);
            }

            completeSequence.OnComplete(async () =>
            {
                ElementGenerator elementGenerator = new ElementGenerator();

                foreach (var gridElement in fallingElements)
                {
                    gridElement.index = Array.IndexOf(_gridElements, gridElement);
                    RectTransform rectTransform = gridElement.GetComponent<RectTransform>();

                    // Animasyon için DoTween kullan
                    rectTransform.DOAnchorPos(_gridElementPositions[gridElement.index], 0.3f).SetEase(Ease.InOutQuad);
                }

                for (int i = 0; i < matchedElements.Count; i++)
                {
                    NumberBehaviour matchedElement = matchedElements[i];
                    matchedElement.gameObject.SetActive(false);
                    matchedElement.transform.position += Vector3.up * 5;

                    matchedElement = elementGenerator.GenerateRandomElement(matchedElement);
                    matchedElement.index = System.Array.IndexOf(_gridElements, matchedElement);
                    RectTransform rectTransform = matchedElements[i].GetComponent<RectTransform>();

                    // Animasyon için DoTween kullan
                    matchedElement.gameObject.SetActive(true);
                    rectTransform.DOAnchorPos(_gridElementPositions[matchedElement.index], Random.Range(0.3f, 0.6f))
                        .SetEase(Ease.InOutQuad);
                }

                
                await Task.Delay(500);
                for (int i = 0; i < matchedElements.Count; i++)
                {
                    Vector2Int gridSize = _levelData.gridSize;
                    int index = matchedElements[i].index;
                    int row = index / gridSize.y;
                    int col = index % gridSize.y;
                    MatchController.Instance.CheckMatch(_gridElements, gridSize, row, col);
                }
                
                for (int i = 0; i < fallingElements.Count; i++)
                {
                    Vector2Int gridSize = _levelData.gridSize;
                    int index = fallingElements[i].index;
                    int row = index / gridSize.y;
                    int col = index % gridSize.y;
                    MatchController.Instance.CheckMatch(_gridElements, gridSize, row, col);
                }
            });
        }

        public void CreateGrid(Vector2Int gridSize, Element[] elements)
        {
            _gridElements = new NumberBehaviour[gridSize.x * gridSize.y];
            _gridElementPositions = new Vector2[gridSize.x * gridSize.y];
            // Clear existing grid
            for (int i = gridParent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(gridParent.GetChild(i).gameObject);
            }

            RectTransform parentRect = gridParent.GetComponent<RectTransform>();
            var rect = parentRect.rect;
            float cellScaleX = rect.width / gridSize.x;
            float cellScaleY = rect.height / gridSize.y;
            float cellScale = Mathf.Min(cellScaleX, cellScaleY);

            // Calculate offset to center grid
            float offsetX = (parentRect.rect.width - gridSize.x * cellScale) / 2.0f;

            // Instantiate cells
            CreateCells(gridSize, cellScale, parentRect, offsetX, elements);
        }

        private void CreateCells(Vector2Int gridSize, float cellScale, RectTransform parentRect, float offsetX,
            Element[] elements)
        {
            ElementGenerator elementGenerator = new ElementGenerator();
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    Element element = elements[y * gridSize.x + x];
                    if(element.selectedNumber == SelectedNumber.Null) continue;
                    
                    float posX = (x * cellScale) + cellScale / 2f - parentRect.rect.width / 2f + offsetX;
                    float posY = -(y * cellScale);

                    NumberBehaviour cell = Instantiate(numberBehaviourPrefab, gridParent);
                    RectTransform cellRect = cell.GetComponent<RectTransform>();
                    cellRect.sizeDelta = new Vector2(cellScale, cellScale);

                    Vector2 cellPositon = new Vector2(posX, posY);
                    cellRect.anchoredPosition = cellPositon;
                    _gridElementPositions[y * gridSize.x + x] = cellPositon;
                    cell.index = y * gridSize.x + x;
                    
                    if (element.selectedNumber != SelectedNumber.Null) // Random Element Creation
                    {
                        cell.Initialize(elementGenerator.GetRandomElementContext(element.selectedNumber),
                            elementGenerator.GetColor(element.selectedNumber), element.selectedNumber);
                        _gridElements[y * gridSize.x + x] = cell;
                    }
                }
            }
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    Element element = elements[y * gridSize.x + x];
                    if(element.selectedNumber != SelectedNumber.Null) continue;
                    
                    float posX = (x * cellScale) + cellScale / 2f - parentRect.rect.width / 2f + offsetX;
                    float posY = -(y * cellScale);

                    NumberBehaviour cell = Instantiate(numberBehaviourPrefab, gridParent);
                    RectTransform cellRect = cell.GetComponent<RectTransform>();
                    cellRect.sizeDelta = new Vector2(cellScale, cellScale);

                    Vector2 cellPositon = new Vector2(posX, posY);
                    cellRect.anchoredPosition = cellPositon;
                    _gridElementPositions[y * gridSize.x + x] = cellPositon;
                    cell.index = y * gridSize.x + x;
                    
                    if (element.selectedNumber == SelectedNumber.Null) // Random Element Creation
                    {
                        cell = elementGenerator.GenerateRandomElement(cell);
                        _gridElements[y * gridSize.x + x] = cell;
                        MatchController.Instance.CheckMatch(_gridElements, gridSize, x, y, true);
                    }
                }
            }
        }

        private void ChangeGridElement(NumberBehaviour numberBehaviour)
        {
            SO_Manager.Load_SO<LevelSignals>().OnDecreaseMoveCount?.Invoke();
            
            ElementGenerator elementGenerator = new ElementGenerator();
            elementGenerator.GenerateRandomElement(numberBehaviour);
            Vector2Int gridSize = _levelData.gridSize;
            int index = Array.IndexOf(_gridElements, numberBehaviour);
            int row = index / gridSize.y;
            int col = index % gridSize.y;
            MatchController.Instance.CheckMatch(_gridElements, gridSize, col, row);
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