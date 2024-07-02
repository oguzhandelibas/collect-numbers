using System;
using UnityEngine;

namespace CollectNumbers
{
    [Serializable]
    public record Goal
    {
        public SelectedColor TargetColor;
        public Material Material;
        public int Count;

        public Color GetColor()
        {
            ColorData colorData = SO_Manager.Load_SO<ColorData>();
            return colorData.Colors[TargetColor].color;
        }
        
        public Goal DeepCopy()
        {
            return new Goal
            {
                TargetColor = this.TargetColor,
                Material = this.Material,
                Count = this.Count
            };
        }
    }
}