using UnityEngine;
using UnityEngine.Events;

namespace CollectNumbers
{
    [CreateAssetMenu(menuName = "Signals/GridSignals", fileName = "SD_GridSignals", order = 0)]
    public class GridSignals : ScriptableObject
    {
        public UnityAction<NumberBehaviour> OnGridElementChanged = delegate { };
    }
}
