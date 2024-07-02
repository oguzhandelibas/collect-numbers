using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace CollectNumbers
{
    public class GoalManager : AbstractSingleton<GoalManager>
    {
        [SerializeField] private GoalPanel goalPanel;
        [SerializeField] private TextMeshProUGUI moveCountText;
        private int _moveCount = 0;
        
        private void Initialize(LevelData levelData)
        {
            _moveCount = levelData.moveCount;
            List<Goal> deepCopyGoals = levelData.Goals.Select(goal => goal.DeepCopy()).ToList();
            goalPanel.CreateGoalIndicators(deepCopyGoals);

            moveCountText.text = _moveCount.ToString();
        }
        
        public async void DecreaseMoveCount()
        {
            _moveCount--;
            if(_moveCount <= 0)
            {
                _moveCount = 0;
                await Task.Delay(500);
                LevelSignals levelSignals = SO_Manager.Load_SO<LevelSignals>();
                levelSignals.OnLevelFinished?.Invoke();
                levelSignals.OnLevelFailed?.Invoke();
            }
            moveCountText.text = _moveCount.ToString();
        }
        
        public void DecreaseGoalCount(SelectedColor selectedColor, GameObject targetObject)
        {
            bool goalsDone = goalPanel.ChangeGoal(selectedColor, targetObject);
            if(GameManager.Instance.gameIsActive && goalsDone)
            {
                LevelSignals levelSignals = SO_Manager.Load_SO<LevelSignals>();
                levelSignals.OnLevelFinished?.Invoke();
                levelSignals.OnLevelSuccessful?.Invoke();
            }
        }
        
        public int GetMoveCount()
        {
            return _moveCount;
        }
        
        #region EVENT SUBSCRIPTION
        
        private void OnEnable()
        {
            LevelSignals levelSignals = SO_Manager.Load_SO<LevelSignals>();
            levelSignals.OnLevelInitialize += Initialize;
        }

        private void OnDisable()
        {
            LevelSignals levelSignals = SO_Manager.Load_SO<LevelSignals>();
            levelSignals.OnLevelInitialize -= Initialize;
        }

        #endregion

        
    }
}
