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
            ColorData colorData = SO_Manager.Load_SO<ColorData>();
            return colorData.Colors[TargetColor].color;
        }
    }
}