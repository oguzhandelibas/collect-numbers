using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CollectNumbers
{
    [Serializable]
    public struct Element
    {
        public SelectedNumber selectedNumber;
        public SelectedColor electedColor;

        public bool hasElement;

        public GUIContent GuiContent;
        public Color Color;
    }
}
