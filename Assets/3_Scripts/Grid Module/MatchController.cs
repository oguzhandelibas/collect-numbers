using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollectNumbers
{
    public class MatchController
    {
        public void CheckMatch(NumberBehaviour[] gridElements, Vector2Int gridSize, int x, int y)
        {
            // AYNI ELEMANLARIN SAYISINI KONTROL EDECEĞİZ
            List<NumberBehaviour> rowElements = new List<NumberBehaviour>();
            List<NumberBehaviour> columnElements = new List<NumberBehaviour>();

            NumberBehaviour currentElement = gridElements[y * gridSize.x + x];

            // Satır elemanlarını kontrol et (x koordinatında sağa ve sola doğru)
            for (int i = -2; i <= 2; i++)
            {
                if (i == 0) continue; // Mevcut elemanı atla

                int checkX = x + i;
                if (checkX >= 0 && checkX < gridSize.x)
                {
                    NumberBehaviour rowElement = gridElements[y * gridSize.x + checkX];
                    if (rowElement != null && rowElement.selectedNumber == currentElement.selectedNumber)
                    {
                        rowElements.Add(rowElement);
                    }
                }
            }

            if (rowElements.Count >= 2)
            {
                Debug.Log("Aynı satırda 3 ve fazlası oldu");
                rowElements.Add(currentElement); // Mevcut elemanı da ekle
                foreach (var behaviour in rowElements)
                {
                    behaviour.gameObject.SetActive(false);
                }
            }

            // Sütun elemanlarını kontrol et (y koordinatında yukarı ve aşağı doğru)
            for (int i = -2; i <= 2; i++)
            {
                if (i == 0) continue; // Mevcut elemanı atla

                int checkY = y + i;
                if (checkY >= 0 && checkY < gridSize.y)
                {
                    NumberBehaviour columnElement = gridElements[checkY * gridSize.x + x];
                    if (columnElement != null && columnElement.selectedNumber == currentElement.selectedNumber)
                    {
                        columnElements.Add(columnElement);
                    }
                }
            }

            if (columnElements.Count >= 2)
            {
                Debug.Log("Aynı sütunda 3 ve fazlası oldu");
                columnElements.Add(currentElement); // Mevcut elemanı da ekle
                foreach (var behaviour in columnElements)
                {
                    behaviour.gameObject.SetActive(false);
                }
            }
        }

        
        public void InitialMatchCheck(NumberBehaviour[] gridElements, Vector2Int gridSize, int x, int y)
        {
            // AYNI ELEMANLARIN SAYISINI KONTROL EDECEĞİZ
            List<NumberBehaviour> rowElements = new List<NumberBehaviour>();
            List<NumberBehaviour> columnElements = new List<NumberBehaviour>();

            NumberBehaviour currentElement = gridElements[y * gridSize.x + x];

            // Satır elemanlarını kontrol et (x koordinatında sağa ve sola doğru)
            for (int i = -2; i <= 2; i++)
            {
                if (i == 0) continue; // Mevcut elemanı atla

                int checkX = x + i;
                if (checkX >= 0 && checkX < gridSize.x)
                {
                    NumberBehaviour rowElement = gridElements[y * gridSize.x + checkX];
                    if (rowElement != null && rowElement.selectedNumber == currentElement.selectedNumber)
                    {
                        rowElements.Add(rowElement);
                    }
                }
            }

            if (rowElements.Count >= 2)
            {
                Debug.Log("Aynı satırda 3 ve fazlası var");
                RegenerateElement(gridElements, currentElement, gridSize);
            }

            // Sütun elemanlarını kontrol et (y koordinatında yukarı ve aşağı doğru)
            for (int i = -2; i <= 2; i++)
            {
                if (i == 0) continue; // Mevcut elemanı atla

                int checkY = y + i;
                if (checkY >= 0 && checkY < gridSize.y)
                {
                    NumberBehaviour columnElement = gridElements[checkY * gridSize.x + x];
                    if (columnElement != null && columnElement.selectedNumber == currentElement.selectedNumber)
                    {
                        columnElements.Add(columnElement);
                    }
                }
            }

            if (columnElements.Count >= 2)
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