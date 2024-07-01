using System.Collections.Generic;
using UnityEngine;

namespace CollectNumbers
{
    public class GoalPanel : MonoBehaviour
    {
        [SerializeField] private LevelData levelData;
        [SerializeField] private Transform indicatorsParent;
        [SerializeField] private GoalBehaviour goalBehaviourPrefab;

        private void Start()
        {
            CreateGoalIndicators(levelData.Goals);
        }

        public void CreateGoalIndicators(List<Goal> goals)
        {
            for (int i = indicatorsParent.childCount - 1; i >= 0; i--)
            {
                Destroy(indicatorsParent.GetChild(i).gameObject);
            }

            for (int i = 0; i < goals.Count; i++)
            {
                GoalBehaviour goal = Instantiate(goalBehaviourPrefab, indicatorsParent);
                goal.Initialize(goals[i].GetColor(), goals[i].Count);
            }
        }
    }
}
