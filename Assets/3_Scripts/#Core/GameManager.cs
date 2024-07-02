using UnityEngine;

namespace CollectNumbers
{
    public class GameManager : AbstractSingleton<GameManager>
    {
        [SerializeField] private LevelData[] levelDatas;
        private LevelSignals _levelSignals;
        public bool gameIsActive = false;
        
        private void Start()
        {
            _levelSignals = SO_Manager.Load_SO<LevelSignals>();
            InitializeNextLevel();
        }
        
        private void InitializeNextLevel()
        {
            gameIsActive = true;
            int levelCount = _levelSignals.OnGetLevelCount() % levelDatas.Length;
            Debug.Log($"Level Initialized in GameManager, LevelCount: {levelCount}");
            _levelSignals.OnLevelInitialize?.Invoke(levelDatas[levelCount]);
        }
        
        private void RestartLevel()
        {
            gameIsActive = true;
            int levelCount = _levelSignals.OnGetLevelCount() % levelDatas.Length;
            Debug.Log($"Level Initialized in GameManager, LevelCount: {levelCount}");
            _levelSignals.OnLevelInitialize?.Invoke(levelDatas[levelCount]);
        }
        
        #region EVENT SUBSCRIPTION
        private void OnEnable()
        {
            LevelSignals levelSignals = SO_Manager.Load_SO<LevelSignals>();
            levelSignals.OnNextLevel += InitializeNextLevel;
            levelSignals.OnRestartLevel += RestartLevel;
        }
        
        private void OnDisable()
        {
            LevelSignals levelSignals = SO_Manager.Load_SO<LevelSignals>();
            levelSignals.OnNextLevel -= InitializeNextLevel;
            levelSignals.OnRestartLevel -= RestartLevel;
        }
        #endregion
    }
}
