using UnityEngine;
using AYellowpaper.SerializedCollections;
using ODProjects.PoolModule.Enums;

namespace ODProjects.PoolModule.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CD_Pool", menuName = "ObjectPooling/CD_Pool", order = 0)]
    public class CD_Pool : ScriptableObject
    {
        [SerializeField] public SerializedDictionary<PoolType, PoolData> PoolDataDictionary = new SerializedDictionary<PoolType, PoolData>();
        public bool HasThisType(PoolType type) => PoolDataDictionary.ContainsKey(type);
    }
}
