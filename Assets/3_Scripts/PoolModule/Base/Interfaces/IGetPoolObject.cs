using UnityEngine;
using ODProjects.PoolModule.Enums;

namespace ODProjects.PoolModule.Interfaces
{
    public interface IGetPoolObject
    {
        GameObject OnGetObjectFromPool(PoolType poolType);
    }
}
