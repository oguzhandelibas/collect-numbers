using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CollectNumbers
{
    public class NumberBehaviour : MonoBehaviour, IPointerDownHandler
    {
        public int index;
        public bool isActive = true;
        public SelectedNumber selectedNumber;
        [SerializeField] private TextMeshProUGUI numberText;
        [SerializeField] private Image numberImage;
    
        public void Initialize(string number, Color color, SelectedNumber selectedNumber)
        {
            this.selectedNumber = selectedNumber;
            numberText.text = number;
            numberImage.color = color;
            isActive = true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if(!GameManager.Instance.gameIsActive || !isActive) return;
            SO_Manager.Load_SO<GridSignals>().OnGridElementChanged?.Invoke(this);
        }
    }
}