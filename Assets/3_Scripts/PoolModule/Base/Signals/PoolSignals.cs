using UnityEngine;
using UnityEngine.Events;
using System;
using ODProjects.PoolModule.Enums;

namespace ODProjects.PoolModule.Signals
{
    [CreateAssetMenu(fileName = "CD_PoolSignals", menuName = "ObjectPooling/CD_PoolSignals", order = 0)]
    public class PoolSignals : ScriptableObject
    {
        public Func<PoolType, GameObject> OnGetObjectFromPool = delegate { return null; };
        public UnityAction<GameObject, PoolType> OnReleaseObjectFromPool = delegate { };
    }
}
