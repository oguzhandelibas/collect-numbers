using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CollectNumbers
{
    public class NumberBehaviour : MonoBehaviour, IPointerDownHandler
    {
        public SelectedNumber selectedNumber;
        [SerializeField] private TextMeshProUGUI numberText;
        [SerializeField] private Image numberImage;
    
        public void Initialize(string number, Color color, SelectedNumber selectedNumber)
        {
            this.selectedNumber = selectedNumber;
            numberText.text = number;
            numberImage.color = color;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SO_Manager.Load_SO<GridSignals>().OnGridElementChanged?.Invoke(this);
        }
    }
}