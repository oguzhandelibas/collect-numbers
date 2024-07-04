using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CollectNumbers
{
    public static class ElementGenerator
    {
        private static T GetRandomEnumValue<T>()
        {
            Array enumValues = Enum.GetValues(typeof(T));
            return (T)enumValues.GetValue(Random.Range(1, enumValues.Length));
        }
        public static Color GetColor(SelectedNumber selectedNumber)
        {
            if (selectedNumber == SelectedNumber.Null) return Color.black;
            ColorData colorData = SO_Manager.Load_SO<ColorData>();
            SelectedColor selectedColor = GetSelectedColor(selectedNumber);
            return colorData.Colors[selectedColor].color;
        }
        
        public static SelectedColor GetSelectedColor(SelectedNumber selectedNumber)
        {
            ColorData colorData = SO_Manager.Load_SO<ColorData>();
            SelectedColor selectedColor = SelectedColor.Null;
            switch (selectedNumber)
            {
                case SelectedNumber.Null:
                    selectedColor = SelectedColor.Null;
                    break;
                case SelectedNumber.One:
                    selectedColor = SelectedColor.Red;
                    break;
                case SelectedNumber.Two:
                    selectedColor = SelectedColor.Green;
                    break;
                case SelectedNumber.Three:
                    selectedColor = SelectedColor.Blue;
                    break;
                case SelectedNumber.Four:
                    selectedColor = SelectedColor.Orange;
                    break;
                case SelectedNumber.Five:
                    selectedColor = SelectedColor.Purple;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return selectedColor;
        }
        
        public static string GetRandomElementContext(SelectedNumber selectedNumber)
        {
            string element = "1";
            switch (selectedNumber)
            {
                case SelectedNumber.One:
                    element = "1";
                    break;
                case SelectedNumber.Two:
                    element = "2";
                    break;
                case SelectedNumber.Three:
                    element = "3";
                    break;
                case SelectedNumber.Four:
                    element = "4";
                    break;
                case SelectedNumber.Five:
                    element = "5";
                    break;
            }
            return element;
        }

        public static NumberBehaviour GetNextElement(NumberBehaviour numberBehaviour)
        {
            Array enumValues = Enum.GetValues(typeof(SelectedNumber));
            int nextElementIndex = (int)numberBehaviour.selectedNumber + 1;
            if (nextElementIndex >= enumValues.Length) nextElementIndex = 0;
            SelectedNumber selectedNumber = (SelectedNumber)enumValues.GetValue(nextElementIndex);
            numberBehaviour.Initialize(GetRandomElementContext(selectedNumber), GetColor(selectedNumber), selectedNumber);
            return numberBehaviour;
        }

        public static NumberBehaviour GenerateRandomElement(NumberBehaviour numberBehaviour)
        {
            SelectedNumber selectedNumber = GetRandomEnumValue<SelectedNumber>();
            while (selectedNumber == numberBehaviour.selectedNumber)
            {
                selectedNumber = GetRandomEnumValue<SelectedNumber>();
            }
            numberBehaviour.Initialize(GetRandomElementContext(selectedNumber), GetColor(selectedNumber), selectedNumber);
            return numberBehaviour;
        }
    }
}
