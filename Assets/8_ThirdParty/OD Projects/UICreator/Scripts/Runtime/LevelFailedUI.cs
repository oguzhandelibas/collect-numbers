using CollectNumbers.UIModule;
using UnityEngine;
using UnityEngine.UI;

namespace CollectNumbers
{
    public class LevelFailedUI : View
    {
        [SerializeField] private Button nextLevelButton;

        private void Start()
        {
            nextLevelButton.onClick.AddListener(NextLevel);
        }

        private void NextLevel()
        {
            SO_Manager.Load_SO<LevelSignals>().OnRestartLevel?.Invoke();
        }
    }
}