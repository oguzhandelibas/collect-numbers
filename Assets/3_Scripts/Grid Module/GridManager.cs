using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CollectNumbers
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        [SerializeField] private NumberBehaviour numberBehaviourPrefab;
        [SerializeField] private Transform gridParent;
        private float _gridOffset;
        public NumberBehaviour[] _gridElements;
        private void Start()
        {
            CreateGrid(levelData.gridSize, levelData.Elements);
        }
        
        
        public void CreateGrid(Vector2Int gridSize, Element[] elements)
        {
            _gridElements = new NumberBehaviour[gridSize.x * gridSize.y];
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

        private void CreateCells(Vector2Int gridSize, float cellScale, RectTransform parentRect, float offsetX, Element[] elements)
        {
            ElementGenerator elementGenerator = new ElementGenerator();
            MatchController matchController = new MatchController();
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {
                    float posX = (x * cellScale) + cellScale / 2f - parentRect.rect.width / 2f + offsetX;
                    float posY = -(y * cellScale);
                    
                    NumberBehaviour cell = Instantiate(numberBehaviourPrefab, gridParent);
                    RectTransform cellRect = cell.GetComponent<RectTransform>();
                    cellRect.sizeDelta = new Vector2(cellScale, cellScale);
                    cellRect.anchoredPosition = new Vector2(posX, posY);

                    Element element = elements[y * gridSize.x + x];
                    if (element.selectedNumber == SelectedNumber.Null) // Random Element Creation
                    {
                        cell = elementGenerator.GenerateRandomElement(cell);
                        _gridElements[y * gridSize.x + x] = cell;
                        matchController.CheckMatches(_gridElements, gridSize, x, y);
                    }
                    else // Set Identified Element
                    {
                        cell.Initialize(elementGenerator.GetRandomElementContext(element.selectedNumber), elementGenerator.GetColor(element.selectedNumber), element.selectedNumber);
                        _gridElements[y * gridSize.x + x] = cell;
                    }
                }
            }
        }
        
    }
}
// Eleman için tek ve sağlam bir class olsun
// Random ve istenen eleman oluşturma tek bir kaynaktan olsun
// Match kontrolü için ayrı bir modül oluşturulmalı
// Match kontrolü istenen fonksiyonda verilen bilgiler doğrultusunda dikey ve yatay kontrol yapmalı