using System.Collections.Generic;
using UnityEngine;

namespace CollectNumbers
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Level/LevelData", order = 1)]
    public class LevelData : ScriptableObject
    {
        public int moveCount;
        public int movementRight;
        public int goalColorCount;
        public Vector2Int gridSize;
        public List<Goal> Goals = new List<Goal>();
        public Element[] Elements = new Element[0];

        public bool HasPath;

        #region GET LEVEL DATA
        public SelectedColor GetSelectedColor(int index) => Elements[index].electedColor;

        public SelectedNumber GetSelectedElement(int index)
        {
            return Elements[index].hasElement ? Elements[index].selectedNumber : SelectedNumber.Null;
        }

        #endregion

        #region LEVEL DATA CREATION
        public void SetArray(int length)
        {
            Elements = new Element[length];
            ClearPath();
        }
        public int ArrayLength() => Elements.Length;
        public bool ElementIsAvailable(int index) => Elements[index].selectedNumber == SelectedNumber.Null;
        public void SetButtonColor(int index, SelectedColor selectedColor, Color color, GUIContent guiContent, SelectedNumber selectedNumber)
        {
            if (!HasPath) HasPath = true;
            Elements[index].Color = color;
            Elements[index].electedColor = selectedColor;
            Elements[index].selectedNumber = selectedNumber;
            Elements[index].GuiContent = guiContent;
            Elements[index].hasElement = true;
        }
        public GUIContent GetContent(int index) => Elements[index].GuiContent;
        public Color GetColor(int index)
        {
            return Elements[index].Color;
        }
        
        public void ClearPath()
        {
            for (int i = 0; i < Elements.Length; i++)
            {
                Elements[i].Color = Color.white;
                Elements[i].GuiContent = new GUIContent("N/A");
                Elements[i].selectedNumber = SelectedNumber.Null;
            }

            HasPath = false;
        }
        public void ResetGrid()
        {
            ClearPath();
            gridSize = new Vector2Int(3,3);
        }

        #endregion
    }
}
