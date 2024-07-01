using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CollectNumbers
{
    [Serializable]
    public struct Element
    {
        [FormerlySerializedAs("SelectedElement")] public SelectedNumber selectedNumber;
        public SelectedColor SelectedColor;

        public bool hasElement;

        public GUIContent GuiContent;
        public Color Color;
    }
}
