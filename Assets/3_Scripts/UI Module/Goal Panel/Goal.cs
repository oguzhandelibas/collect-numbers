using System;
using UnityEngine;

namespace CollectNumbers
{
    [Serializable]
    public record Goal
    {
        public SelectedColor TargetColor;
        public int Count;

        public Color GetColor()
        {
            Color color = Color.white;
            switch (TargetColor)
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
    }
}