using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CollectNumbers
{
    public class GridManager : AbstractSingleton<GridManager>
    {
        [SerializeField] private LevelData levelData;
        [SerializeField] private NumberBehaviour numberBehaviourPrefab;
        [SerializeField] private Transform gridParent;
        private float _gridOffset;
        public NumberBehaviour[] _gridElements;
        public Vector2[] _gridElementPositions;

        private void Start()
        {
            CreateGrid(levelData.gridSize, levelData.Elements);
        }

        public void SetPositions()
        {
            foreach (var gridElement in _gridElements)
            {
                gridElement.index = Array.IndexOf(_gridElements, gridElement);
                RectTransform rectTransform = gridElement.GetComponent<RectTransform>();
                
                // Animasyon için DoTween kullan
                rectTransform.DOAnchorPos(_gridElementPositions[gridElement.index], Random.Range(0.3f, 0.6f)).SetEase(Ease.InOutQuad);
                gridElement.gameObject.SetActive(true);
            }
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
                    float posX = (x * cellScale) + cellScale / 2f - parentRect.rect.width / 2f + offsetX;
                    float posY = -(y * cellScale);

                    NumberBehaviour cell = Instantiate(numberBehaviourPrefab, gridParent);
                    RectTransform cellRect = cell.GetComponent<RectTransform>();
                    cellRect.sizeDelta = new Vector2(cellScale, cellScale);

                    Vector2 cellPositon = new Vector2(posX, posY);
                    cellRect.anchoredPosition = cellPositon;
                    _gridElementPositions[y * gridSize.x + x] = cellPositon;
                    cell.index = y * gridSize.x + x;

                    Element element = elements[y * gridSize.x + x];
                    if (element.selectedNumber == SelectedNumber.Null) // Random Element Creation
                    {
                        cell = elementGenerator.GenerateRandomElement(cell);
                        _gridElements[y * gridSize.x + x] = cell;
                        MatchController.Instance.InitialMatchCheck(_gridElements, gridSize, x, y);
                    }
                    else // Set Identified Element
                    {
                        cell.Initialize(elementGenerator.GetRandomElementContext(element.selectedNumber),
                            elementGenerator.GetColor(element.selectedNumber), element.selectedNumber);
                        _gridElements[y * gridSize.x + x] = cell;
                    }
                }
            }
        }

        private void ChangeGridElement(NumberBehaviour numberBehaviour)
        {
            ElementGenerator elementGenerator = new ElementGenerator();
            elementGenerator.GenerateRandomElement(numberBehaviour);
            Vector2Int gridSize = levelData.gridSize;
            int index = Array.IndexOf(_gridElements, numberBehaviour);
            int row = index / gridSize.y;
            int col = index % gridSize.y;
            MatchController.Instance.CheckMatch(_gridElements, gridSize, col, row);
        }

        #region EVENT SUBSCRIPTION

        private void OnEnable()
        {
            SO_Manager.Load_SO<GridSignals>().OnGridElementChanged += ChangeGridElement;
        }

        private void OnDisable()
        {
            SO_Manager.Load_SO<GridSignals>().OnGridElementChanged -= ChangeGridElement;
        }

        #endregion
    }
}