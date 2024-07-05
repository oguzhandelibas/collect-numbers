using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CollectNumbers
{
    public static class MatchController
    {
        private static List<NumberBehaviour> _matchedNumbers = new List<NumberBehaviour>();
        private static List<NumberBehaviour> _fallingNumbers = new List<NumberBehaviour>();
        private static bool _isOnProccess = false;
        
        public static bool FindAllMatches(NumberBehaviour[,] gridElements, bool initial)
        {
            if (!GameManager.Instance.gameIsActive || _isOnProccess) return false;
            
            if(!initial)_isOnProccess = true;
            
            Vector2Int gridSize = new Vector2Int(gridElements.GetLength(0), gridElements.GetLength(1));
            _matchedNumbers.Clear();
            _fallingNumbers.Clear();

            List<NumberBehaviour> verticalCheckNumbers = new List<NumberBehaviour>();
            List<NumberBehaviour> horizontalCheckNumbers = new List<NumberBehaviour>();
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    NumberBehaviour currentNum = gridElements[x, y];
                    if (currentNum != null && currentNum.selectedNumber != SelectedNumber.Null)
                    {
                        // VERTICAL CONTROL
                        if (y > 0 && y < gridSize.y - 1)
                        {
                            NumberBehaviour aboveNum = gridElements[x, y + 1];
                            NumberBehaviour belowNum = gridElements[x, y - 1];
                            if (aboveNum != null && belowNum != null)
                            {
                                if (aboveNum.selectedNumber == currentNum.selectedNumber && belowNum.selectedNumber == currentNum.selectedNumber)
                                {
                                    if (initial)
                                    {
                                        if (!currentNum.isHolded) currentNum = ElementGenerator.GenerateRandomElement(currentNum);
                                        else if (!aboveNum.isHolded) aboveNum = ElementGenerator.GenerateRandomElement(aboveNum);
                                        else if (!belowNum.isHolded) belowNum = ElementGenerator.GenerateRandomElement(belowNum);
                                    }
                                    else
                                    {
                                        verticalCheckNumbers.Add(currentNum);
                                        verticalCheckNumbers.Add(aboveNum);
                                        verticalCheckNumbers.Add(belowNum);
                                    }

                                    Debug.Log("Vertical Matched Numbers: " + currentNum.selectedNumber + " " + aboveNum.selectedNumber + " " + belowNum.selectedNumber);
                                }
                            }
                        }

                        // HORIZONTAL CONTROL
                        if (x > 0 && x < gridSize.x - 1)
                        {
                            NumberBehaviour leftNum = gridElements[x - 1, y];
                            NumberBehaviour rightNum = gridElements[x + 1, y];
                            //if (leftGem == null || rightGem == null) return;
                            if (leftNum != null && rightNum != null)
                            {
                                if (leftNum.selectedNumber == currentNum.selectedNumber &&
                                    rightNum.selectedNumber == currentNum.selectedNumber)
                                {
                                    if (initial)
                                    {
                                        if (!currentNum.isHolded) currentNum = ElementGenerator.GenerateRandomElement(currentNum);
                                        else if (!leftNum.isHolded) leftNum = ElementGenerator.GenerateRandomElement(leftNum);
                                        else if (!rightNum.isHolded) rightNum = ElementGenerator.GenerateRandomElement(rightNum);
                                    }
                                    else
                                    {
                                        horizontalCheckNumbers.Add(currentNum);
                                        horizontalCheckNumbers.Add(leftNum);
                                        horizontalCheckNumbers.Add(rightNum);
                                    }

                                    Debug.Log("Horizontal Matched Numbers: " + currentNum.selectedNumber + " " +
                                              leftNum.selectedNumber + " " + rightNum.selectedNumber);
                                }
                            }
                        }
                    }
                }
            }
            bool hasMatch = verticalCheckNumbers.Count > 0 || horizontalCheckNumbers.Count > 0;
            if (!initial)
            {
                if(verticalCheckNumbers.Count>0) AddToMatchedNumbers(gridElements, verticalCheckNumbers.ToArray(), true);
                if(horizontalCheckNumbers.Count>0) AddToMatchedNumbers(gridElements, horizontalCheckNumbers.ToArray(), false);
                
                if(hasMatch) GridManager.Instance.SetPositions(_matchedNumbers, _fallingNumbers);
                _isOnProccess = false;
            }

            return hasMatch;
        }

        private static void AddToMatchedNumbers(NumberBehaviour[,] gridElements, NumberBehaviour[] numberBehaviours, bool isVertical)
        {
            int smallestNum = numberBehaviours.OrderBy(nb => nb.index.y).FirstOrDefault()!.index.y;
            if(!isVertical)
            {
                smallestNum = numberBehaviours.OrderBy(nb => nb.index.y).LastOrDefault()!.index.y;
            }
            
            int target = 0;
            List<NumberBehaviour> matchedTemp = new List<NumberBehaviour>();
            foreach (var num in numberBehaviours)
            {
                if (!_matchedNumbers.Contains(num))
                {
                    target++;
                    matchedTemp.Add(num);
                    _matchedNumbers.Add(num);
                    int targetYIndex = num.index.y - smallestNum;
                    if (targetYIndex < 0)
                    {
                        smallestNum--;
                        targetYIndex = 0;
                    }
                    gridElements[num.index.x, num.index.y].index  = new Vector2Int(num.index.x, targetYIndex);
                }
            }
            int indexTarget = isVertical ? target : 1;
            foreach (var num in matchedTemp)
            {
                for (int i = 0; i < smallestNum; i++)
                {
                    if (!_fallingNumbers.Contains(gridElements[num.index.x, i]) && !matchedTemp.Contains(gridElements[num.index.x, i]) && gridElements[num.index.x, i] != null)
                    {
                        _fallingNumbers.Add(gridElements[num.index.x, i]);
                        gridElements[num.index.x, i].index = new Vector2Int(num.index.x, i + indexTarget);
                    }
                }
            }
        }
        
        public static bool CheckForDuplicateIndices(NumberBehaviour[,] gridElements)
        {
            HashSet<Vector2Int> seenIndices = new HashSet<Vector2Int>();
            List<Vector2Int> duplicateIndices = new List<Vector2Int>();

            int rows = gridElements.GetLength(0);
            int cols = gridElements.GetLength(1);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Vector2Int currentIndex = gridElements[row, col].index;
                
                    if (seenIndices.Contains(currentIndex))
                    {
                        duplicateIndices.Add(currentIndex);
                    }
                    else
                    {
                        seenIndices.Add(currentIndex);
                    }
                }
            }

            return duplicateIndices.Count > 0;
        }
    }
}