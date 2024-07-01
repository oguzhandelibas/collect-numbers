using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CollectNumbers.LevelModule
{
    public class LevelSignals : MonoBehaviour
    {
        public UnityAction onLevelInitialize = delegate { };
        public UnityAction onLevelSuccessful = delegate { };
        public UnityAction onNextLevel = delegate { };
        public UnityAction onRestartLevel = delegate { };

        public Func<int> onGetLevelCount = delegate { return 0; };
        public Func<Vector2Int> onGetLevelGridSize = delegate { return default; };
    }
}
