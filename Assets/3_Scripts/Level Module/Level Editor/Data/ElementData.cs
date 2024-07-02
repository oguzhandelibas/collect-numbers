using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace CollectNumbers
{
    [CreateAssetMenu(fileName = "ElementData", menuName = "ScriptableObjects/ElementData", order = 1)]
    public class ElementData : ScriptableObject
    {
        [SerializedDictionary("Element Type", "Texture")]
        public SerializedDictionary<SelectedNumber, Goal> Elements;
    }
}
