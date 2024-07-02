using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CollectNumbers
{
    [CreateAssetMenu(menuName = "Signals/LevelSignals", fileName = "SD_LevelSignals", order = 0)]
    public class LevelSignals : ScriptableObject
    {
        public UnityAction<LevelData> OnLevelInitialize = delegate { };
        public UnityAction OnLevelSuccessful = delegate { };
        public UnityAction OnLevelFailed = delegate { };
        public UnityAction OnNextLevel = delegate { };
        public UnityAction OnRestartLevel = delegate { };
        public UnityAction OnLevelFinished = delegate { };
        public UnityAction OnDecreaseMoveCount = delegate { };
        
        public Func<int> OnGetLevelCount = delegate { return 0; };
    }
}
