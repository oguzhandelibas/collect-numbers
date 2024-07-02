using CollectNumbers.UIModule;
using UnityEngine;

namespace CollectNumbers
{
    public class LevelManager : AbstractSingleton<LevelManager>
    {
        private int _currentLevelIndex;

        private void LevelInitialize(LevelData levelData)
        {
            Debug.Log("Level Initialized");
            UIManager.Instance.Show<GameUI>();
        }
        
        private void LevelFinished()
        {
            Debug.Log("Level Finished");
            GameManager.Instance.gameIsActive = false;
        }
        
        private int GetLevelCount()
        {
            return _currentLevelIndex;
        }

        private void NextLevel()
        {
            Debug.Log("Next Level");
            _currentLevelIndex++;
            
            UIManager.Instance.Show<GameUI>();
        }
        
        private void LevelSuccessful()
        {
            Debug.Log("Level Successful");
            UIManager.Instance.Show<LevelCompletedUI>();
            _currentLevelIndex++;
        }
        
        private void LevelFailed()
        {
            UIManager.Instance.Show<LevelFailedUI>();
        }
        
        private void RestartLevel()
        {
            Debug.Log("Restart Level");
        }

        private void DecreaseMoveCount()
        {
            GoalManager.Instance.DecreaseMoveCount();
        }
        
        
        #region EVENT SUBSCRIPTION
        
        private void OnEnable()
        {
            LevelSignals levelSignals = SO_Manager.Load_SO<LevelSignals>();
            levelSignals.OnLevelInitialize += LevelInitialize;
            levelSignals.OnLevelFinished += LevelFinished;
            levelSignals.OnLevelSuccessful += LevelSuccessful;
            levelSignals.OnLevelFailed += LevelFailed;
            levelSignals.OnRestartLevel += RestartLevel;
            levelSignals.OnNextLevel += NextLevel;
            levelSignals.OnGetLevelCount += GetLevelCount;
            levelSignals.OnDecreaseMoveCount += DecreaseMoveCount;
        }

        

        private void OnDisable()
        {
            LevelSignals levelSignals = SO_Manager.Load_SO<LevelSignals>();
            levelSignals.OnLevelInitialize -= LevelInitialize;
            levelSignals.OnLevelFinished -= LevelFinished;
            levelSignals.OnLevelSuccessful -= LevelSuccessful;
            levelSignals.OnLevelFailed -= LevelFailed;
            levelSignals.OnRestartLevel -= RestartLevel;
            levelSignals.OnNextLevel -= NextLevel;
            levelSignals.OnGetLevelCount -= GetLevelCount;
            levelSignals.OnDecreaseMoveCount -= DecreaseMoveCount;
        }

        #endregion
    }
}
