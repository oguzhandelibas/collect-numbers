using System;
using UnityEngine;
using UnityEngine.UI;

namespace CollectNumbers.UIModule
{
    public class LevelCompletedUI : View
    {
        [SerializeField] private Button nextLevelButton;

        private void Start()
        {
            nextLevelButton.onClick.AddListener(NextLevel);
        }

        private void NextLevel()
        {
            SO_Manager.Load_SO<LevelSignals>().OnNextLevel?.Invoke();
        }
    }
}
