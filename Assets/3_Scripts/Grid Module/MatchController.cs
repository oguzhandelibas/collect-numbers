using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CollectNumbers
{
    public class MatchController : AbstractSingleton<MatchController>
    {
        //TOPLU MATCH İÇİN Bİ DÖNGÜ YETERLİ ABEM 0'DAN MAX'A KADAR KONTROL ETTİR İŞTE
        public void CheckAllMatch(NumberBehaviour[] gridElements, Vector2Int gridSize)
        {
            
        }
        
        public void CheckMatch(NumberBehaviour[] gridElements, Vector2Int gridSize, int x, int y, bool initial = false)
        {
            // AYNI ELEMANLARIN SAYISINI KONTROL EDECEĞİZ
            List<NumberBehaviour> rowElements = new List<NumberBehaviour>();
            List<NumberBehaviour> columnElements = new List<NumberBehaviour>();
            NumberBehaviour currentElement = gridElements[y * gridSize.x + x];

            // Satır elemanlarını kontrol et (x koordinatında sağa ve sola doğru)
            for (int direction = -1; direction <= 1; direction +=2)
            {
                bool previousMatch = false;
                int checkX = x + direction;
                if (checkX >= 0 && checkX < gridSize.x)
                {
                    NumberBehaviour rowElement = gridElements[y * gridSize.x + checkX];
                    if (rowElement != null && rowElement.selectedNumber == currentElement.selectedNumber)
                    {
                        rowElements.Add(rowElement);
                        previousMatch = true;
                    }
                }

                // Eğer ilk birimde eşleşme varsa, ikinci birimi kontrol et
                if (previousMatch)
                {
                    checkX = x + 2 * direction;
                    if (checkX >= 0 && checkX < gridSize.x)
                    {
                        NumberBehaviour rowElement = gridElements[y * gridSize.x + checkX];
                        if (rowElement != null && rowElement.selectedNumber == currentElement.selectedNumber)
                        {
                            rowElements.Add(rowElement);
                        }
                    }
                }
            }
            if (rowElements.Count >= 2)
            {
                if (initial) { RegenerateElement(gridElements, currentElement, gridSize); return; }
                
                rowElements.Add(currentElement); // Mevcut elemanı da ekle
                List<int> matchIndexes = new List<int>();
                List<int> willFallIndexes = new List<int>();
                foreach (var behaviour in rowElements)
                {
                    matchIndexes.Add(behaviour.index);
                    for (int i = y; i > 0; i--)
                    { willFallIndexes.Add((behaviour.index) - (i * gridSize.y)); }
                }

                NumberBehaviour[] matchArray = new NumberBehaviour[matchIndexes.Count];
                NumberBehaviour[] willFallArray = new NumberBehaviour[willFallIndexes.Count];
                for (int i = 0; i < matchIndexes.Count; i++)
                { matchArray[i] = gridElements[matchIndexes[i]]; }
                
                for (int i = willFallIndexes.Count-1; i >= 0 ; i--)
                {
                    gridElements[willFallIndexes[i] + gridSize.y] = gridElements[willFallIndexes[i]];
                    willFallArray[i] = gridElements[willFallIndexes[i]];
                }
                
                for (int i = 0; i < matchIndexes.Count; i++)
                { gridElements[matchIndexes[i] - y * gridSize.y] = matchArray[i]; }
                
                GridManager.Instance.SetPositions(matchArray.ToList(), willFallArray.ToList());
            }

            // Sütun elemanlarını kontrol et (y koordinatında yukarı ve aşağı doğru)
            for (int direction = -1; direction <= 1; direction += 2)
            {
                bool firstMatch = false;
                int checkY = y + direction;
                if (checkY >= 0 && checkY < gridSize.y)
                {
                    NumberBehaviour columnElement = gridElements[checkY * gridSize.x + x];
                    if (columnElement != null && columnElement.selectedNumber == currentElement.selectedNumber)
                    {
                        columnElements.Add(columnElement);
                        firstMatch = true;
                    }
                }

                // Eğer ilk birimde eşleşme varsa, ikinci birimi kontrol et
                if (firstMatch)
                {
                    checkY = y + 2 * direction;
                    if (checkY >= 0 && checkY < gridSize.y)
                    {
                        NumberBehaviour columnElement = gridElements[checkY * gridSize.x + x];
                        if (columnElement != null && columnElement.selectedNumber == currentElement.selectedNumber)
                        {
                            columnElements.Add(columnElement);
                        }
                    }
                }
            }
            if (columnElements.Count >= 2)
            {
                if (initial) { RegenerateElement(gridElements, currentElement, gridSize); return; }
                
                columnElements.Add(currentElement); // Mevcut elemanı da ekle
                int smallest = 99;
                List<int> willFallIndexes = new List<int>();
                List<int> matchIndexes = new List<int>();
                
                foreach (var behaviour in columnElements)
                {
                    matchIndexes.Add(behaviour.index);
                    smallest = Mathf.Min(smallest, behaviour.index);
                }
                
                for (int i = (int)(smallest / gridSize.y); i > 0; i--)
                { willFallIndexes.Add((gridElements[smallest].index) - (i * gridSize.y)); }

                NumberBehaviour[] willFallArray = new NumberBehaviour[willFallIndexes.Count];
                NumberBehaviour[] matchArray = new NumberBehaviour[matchIndexes.Count];
                for (int i = 0; i < matchIndexes.Count; i++)
                { matchArray[i] = gridElements[matchIndexes[i]]; }
                
                for (int i = willFallIndexes.Count-1; i >= 0 ; i--)
                {
                    gridElements[willFallIndexes[i] + gridSize.y * columnElements.Count] = gridElements[willFallIndexes[i]];
                    willFallArray[i] = gridElements[willFallIndexes[i]];
                }

                for (int i = 0; i < matchIndexes.Count; i++)
                { gridElements[matchIndexes[i] - (int)(smallest / gridSize.y) * gridSize.y] = matchArray[i]; }
                
                GridManager.Instance.SetPositions(matchArray.ToList(), willFallArray.ToList());
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

            element.Initialize(elementGenerator.GetRandomElementContext(newNumber),
                elementGenerator.GetColor(newNumber),
                newNumber);
        }
        private bool IsDuplicateInRowOrColumn(NumberBehaviour[] gridElements, NumberBehaviour element, SelectedNumber newNumber, Vector2Int gridSize)
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