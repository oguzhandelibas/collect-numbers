using System;
using UnityEngine;

namespace ODProjects.PoolModule.Data
{
    [Serializable]
    public struct PoolData
    {
        public GameObject ObjectType;
        public int initalAmount;
        public bool isDynamic;
    }
}
