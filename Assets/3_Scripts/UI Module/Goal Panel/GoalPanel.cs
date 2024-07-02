using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace CollectNumbers
{
    public class GoalPanel : MonoBehaviour
    {
        [SerializeField] private Transform indicatorsParent;
        [SerializeField] private GoalBehaviour goalBehaviourPrefab;
        private List<Goal> _goals = new List<Goal>();
        private GoalBehaviour[] _goalBehaviours;
        
        public void CreateGoalIndicators(List<Goal> goals)
        {
            for (int i = indicatorsParent.childCount - 1; i >= 0; i--)
            {
                Destroy(indicatorsParent.GetChild(i).gameObject);
            }
            
            _goals = goals;
            _goalBehaviours = new GoalBehaviour[_goals.Count];
            foreach (var t in goals)
            {
                GoalBehaviour goal = Instantiate(goalBehaviourPrefab, indicatorsParent);
                goal.Initialize(t.GetColor(), t.Count);
                _goalBehaviours[_goals.IndexOf(t)] = goal;
            }
        }

        public bool ChangeGoal(SelectedColor selectedColor, GameObject targetObject)
        {
            if (_goals.All(x => x.TargetColor != selectedColor)) return false;

            Goal goal = _goals.Find(x => x.TargetColor == selectedColor);
            goal.Count--;
            if(goal.Count <= 0) goal.Count = 0;
            _goalBehaviours[_goals.IndexOf(goal)].Initialize(goal.GetColor(), goal.Count);
    
            if (goal.Count <= 0)
            {
                Color goalColor = goal.GetColor();
                goalColor = new Color(goalColor.r, goalColor.g, goalColor.b, 0.5f);
                _goalBehaviours[_goals.IndexOf(goal)].Initialize(goalColor, goal.Count);
                goal.Count = 0;
            }

            bool isFinished = _goals.All(g => g.Count <= 0);

            // Instantiate ve hareket ettirme işlemi
            GameObject newObject = Instantiate(targetObject, targetObject.transform.parent);
            Vector3 targetPosition = _goalBehaviours[_goals.IndexOf(goal)].transform.position;
            newObject.transform.position = targetObject.transform.position; // Başlangıç pozisyonu

            Sequence sequence = DOTween.Sequence();
            sequence.Append(newObject.transform.DOMove(targetPosition, 0.5f).SetEase(Ease.InOutQuad))
                .Join(newObject.transform.DOScale(newObject.transform.localScale * 0.4f, 0.5f).SetEase(Ease.InOutQuad))
                .OnComplete(() =>
                {
                    Destroy(newObject);
                });
            
            return isFinished;
        }

        
    }
}
