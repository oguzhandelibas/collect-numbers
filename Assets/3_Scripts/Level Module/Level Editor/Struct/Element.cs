using System;
using UnityEngine;

namespace CollectNumbers
{
    [Serializable]
    public struct Element
    {
        public SelectedElement SelectedElement;
        public SelectedColor SelectedColor;

        public bool hasElement;

        public GUIContent GuiContent;
        public Color Color;
        
        public Color GetColor()
        {
            Color color = Color.white;
            switch (SelectedColor)
            {
                case SelectedColor.Red:
                    color = Color.red;
                    break;
                case SelectedColor.Green:
                    color = Color.green;
                    break;
                case SelectedColor.Blue:
                    color = Color.blue;
                    break;
                case SelectedColor.Orange:
                    color = Color.yellow;
                    break;
                case SelectedColor.Purple:
                    color = Color.magenta;
                    break;
            }
            return color;
        }
        
        public string GetElementContext()
        {
            string element = "1";
            switch (SelectedElement)
            {
                case SelectedElement.One:
                    element = "1";
                    break;
                case SelectedElement.Two:
                    element = "2";
                    break;
                case SelectedElement.Three:
                    element = "3";
                    break;
                case SelectedElement.Four:
                    element = "4";
                    break;
                case SelectedElement.Five:
                    element = "5";
                    break;
            }
            return element;
        }
    }
}
