using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async void SetPositions(List<NumberBehaviour> matchedElements, List<NumberBehaviour> fallingElements)
        {
        }

        public void CreateGrid(Vector2Int gridSize, Element[] elements)
        {
            MatchController matchController = new MatchController();
            ElementGenerator elementGenerator = new ElementGenerator();
            
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
                    _gridElementPositions[x,y] = cellPositon;
                    numberBehaviour.index = y * gridSize.x + x;

                    numberBehaviour.selectedNumber = elements2D[x, y].selectedNumber;

                    if (numberBehaviour.selectedNumber != SelectedNumber.Null)
                    {
                        numberBehaviour.Initialize(elementGenerator.GetRandomElementContext(elements2D[x,y].selectedNumber),
                            elementGenerator.GetColor(elements2D[x,y].selectedNumber), elements2D[x,y].selectedNumber, _levelData.movementRight);
                    }
                    else
                    {
                        numberBehaviour = elementGenerator.GenerateRandomElement(numberBehaviour, 0);
                        matchController.CheckSpecialMatch(_gridElements, gridSize, x, y);
                    }
                }
            }
            
            //matchController.FindAllMatches(_gridElements, gridSize);
        }

        #region Helper Methods

        private Element[,] ConvertTo2DArray(Element[] elements, int rowCount, int columnCount)
        {
            Element[,] elements2d = new Element[rowCount, columnCount];
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    elements2d[j, i] = elements[i * columnCount + j];
                }
            }
            return elements2d;
        }
        
        #endregion
        

        private void ChangeGridElement(NumberBehaviour numberBehaviour)
        {
            
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