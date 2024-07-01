using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace CollectNumbers
{
    [CreateAssetMenu(fileName = "ColorData", menuName = "ScriptableObjects/ColorData", order = 1)]
    public class ColorData : ScriptableObject
    {
        [SerializedDictionary("Color Type", "Color")]
        public SerializedDictionary<SelectedColor, Material> Colors;
    }
}
