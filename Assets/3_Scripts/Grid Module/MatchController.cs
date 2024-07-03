using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CollectNumbers
{
    public class MatchController
    {
        private List<NumberBehaviour> _matchedNumbers = new List<NumberBehaviour>();

        public void CheckSpecialMatch(NumberBehaviour[,] gridElements, Vector2Int gridSize, int x, int y)
        {
            ElementGenerator elementGenerator = new ElementGenerator();
            NumberBehaviour currentNum = gridElements[x, y];
            if (currentNum != null)
            {
                // LEFT & RIGHT CONTROL
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
                            Debug.Log("H Match Var");
                            currentNum = elementGenerator.GenerateRandomElement(currentNum, 0);
                        }
                    }
                }

                // ABOVE & BELOW CONTROL
                if (y > 0 && y < gridSize.y - 1)
                {
                    NumberBehaviour aboveNum = gridElements[x, y + 1];
                    NumberBehaviour belowNum = gridElements[x, y - 1];
                    if (aboveNum != null && belowNum != null)
                    {
                        if (aboveNum.selectedNumber == currentNum.selectedNumber &&
                            belowNum.selectedNumber == currentNum.selectedNumber)
                        {
                            Debug.Log("V Match Var");
                            currentNum = elementGenerator.GenerateRandomElement(currentNum, 0);
                        }
                    }
                }
            }
        }

        public void FindAllMatches(NumberBehaviour[,] gridElements, Vector2Int gridSize)
        {
            if (!GameManager.Instance.gameIsActive) return;
            ElementGenerator elementGenerator = new ElementGenerator();

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    NumberBehaviour currentNum = gridElements[x, y];
                    if (currentNum != null)
                    {
                        // LEFT & RIGHT CONTROL
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
                                    if (!_matchedNumbers.Contains(currentNum)) _matchedNumbers.Add(currentNum);
                                    if (!_matchedNumbers.Contains(leftNum)) _matchedNumbers.Add(leftNum);
                                    if (!_matchedNumbers.Contains(rightNum)) _matchedNumbers.Add(rightNum);
                                    Debug.Log("Horizontal Matched Numbers: " + currentNum.selectedNumber + " " +
                                              leftNum.selectedNumber + " " + rightNum.selectedNumber);
                                }
                            }
                        }

                        // ABOVE & BELOW CONTROL
                        if (y > 0 && y < gridSize.y - 1)
                        {
                            NumberBehaviour aboveNum = gridElements[x, y + 1];
                            NumberBehaviour belowNum = gridElements[x, y - 1];
                            if (aboveNum != null && belowNum != null)
                            {
                                if (aboveNum.selectedNumber == currentNum.selectedNumber &&
                                    belowNum.selectedNumber == currentNum.selectedNumber)
                                {
                                    if (!_matchedNumbers.Contains(currentNum)) _matchedNumbers.Add(currentNum);
                                    if (!_matchedNumbers.Contains(aboveNum)) _matchedNumbers.Add(aboveNum);
                                    if (!_matchedNumbers.Contains(belowNum)) _matchedNumbers.Add(belowNum);

                                    Debug.Log("Vertical Matched Numbers: " + currentNum.selectedNumber + " " +
                                              aboveNum.selectedNumber + " " + belowNum.selectedNumber);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}