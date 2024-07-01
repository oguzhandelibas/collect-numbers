using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollectNumbers
{
    public class MatchController
    {
        public void CheckMatches(NumberBehaviour[] gridElements, Vector2Int gridSize, int x, int y)
        {
            // AYNI ELEMANLARIN SAYISINI KONTROL EDECEĞİZ
            List<NumberBehaviour> rowElements = new List<NumberBehaviour>();
            List<NumberBehaviour> columnElements = new List<NumberBehaviour>();

            NumberBehaviour currentElement = gridElements[y * gridSize.x + x];

            // Satır elemanlarını kontrol et
            for (int i = 0; i < gridSize.x; i++)
            {
                NumberBehaviour rowElement = gridElements[y * gridSize.x + i];
                if (rowElement == null) continue;

                if (rowElement.selectedNumber == currentElement.selectedNumber)
                {
                    rowElements.Add(rowElement);
                }
            }

            if (rowElements.Count > 2)
            {
                Debug.Log("Aynı satırda 3 ve fazlası var");
                RegenerateElement(gridElements, currentElement, gridSize);
            }

            // Sütun elemanlarını kontrol et
            for (int i = 0; i < gridSize.y; i++)
            {
                NumberBehaviour columnElement = gridElements[i * gridSize.x + x];
                if (columnElement == null) continue;

                if (columnElement.selectedNumber == currentElement.selectedNumber)
                {
                    columnElements.Add(columnElement);
                }
            }

            if (columnElements.Count > 2)
            {
                Debug.Log("Aynı sütunda 3 ve fazlası var");
                RegenerateElement(gridElements, currentElement, gridSize);
            }
        }

        private void RegenerateElement(NumberBehaviour[] gridElements, NumberBehaviour element, Vector2Int gridSize)
        {
            SelectedNumber newNumber;
            ElementGenerator elementGenerator = new ElementGenerator();
            do
            {
                newNumber = elementGenerator.GetRandomEnumValue<SelectedNumber>();
            } while (IsDuplicateInRowOrColumn(gridElements, element, newNumber, gridSize));

            element.Initialize(elementGenerator.GetRandomElementContext(newNumber), elementGenerator.GetColor(newNumber),
                newNumber);
        }

        private bool IsDuplicateInRowOrColumn(NumberBehaviour[] gridElements, NumberBehaviour element, SelectedNumber newNumber,
            Vector2Int gridSize)
        {
            int index = Array.IndexOf(gridElements, element);
            int x = index % gridSize.x;
            int y = index / gridSize.x;

            // Satır kontrolü
            for (int i = 0; i < gridSize.x; i++)
            {
                if (gridElements[y * gridSize.x + i] != null &&
                    gridElements[y * gridSize.x + i].selectedNumber == newNumber)
                {
                    return true;
                }
            }

            // Sütun kontrolü
            for (int i = 0; i < gridSize.y; i++)
            {
                if (gridElements[i * gridSize.x + x] != null &&
                    gridElements[i * gridSize.x + x].selectedNumber == newNumber)
                {
                    return true;
                }
            }

            return false;
        }
    }
}