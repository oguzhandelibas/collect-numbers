using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
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
            Debug.Log("Selam");
        }

        private void OnMouseDown()
        {
            Debug.Log("naber");
        }
    }
}