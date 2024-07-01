using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CollectNumbers
{
    public class GoalBehaviour : MonoBehaviour
    {
        [SerializeField] private Image goalImage;
        [SerializeField] private TextMeshProUGUI goalCountText;

        public void Initialize(Color goalColor, int goalCount)
        {
            goalImage.color = goalColor;
            goalCountText.text = goalCount.ToString();
        }
    
        public void UpdateGoalCount(int goalCount)
        {
            goalCountText.text = goalCount.ToString();
        }
    }
}
