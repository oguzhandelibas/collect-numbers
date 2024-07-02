using UnityEngine;
using ODProjects.PoolModule.Enums;

namespace ODProjects.PoolModule.Interfaces
{
    public interface IReleasePoolObject
    {
        void OnReleaseObjectFromPool(GameObject obj, PoolType poolType);
    }
}
