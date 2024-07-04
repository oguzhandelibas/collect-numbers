using DG.Tweening;
using MoreMountains.NiceVibrations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CollectNumbers
{
    public class NumberBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public Vector2Int index;
        public bool isHolded = false;
        public bool isActive = true;
        public SelectedNumber selectedNumber;
        [SerializeField] private TextMeshProUGUI numberText;
        [SerializeField] private Image numberImage;
        [SerializeField] private GameObject effectPrefab;
        
        private Vector3 _originalScale;
        private Tween _downTween, _upTween;
        
        public void Initialize(string number, Color color, SelectedNumber selectedNumber)
        {
            this.selectedNumber = selectedNumber;
            if(this.selectedNumber == SelectedNumber.Null) numberText.text = "";
            else numberText.text = number;
            numberImage.color = color;
            isActive = true;
            _originalScale = new Vector3(1,1,1);
        }
        

        public void Explode(bool activeness)
        {
            effectPrefab.SetActive(activeness);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            MMVibrationManager.Haptic(HapticTypes.MediumImpact, false, true, this);
            if(selectedNumber == SelectedNumber.Null) return;
            
            if(!GameManager.Instance.gameIsActive || !isActive)
            {
                _downTween.Kill();
                _upTween.Kill();
                return;
            }
            _downTween = transform.DOScale(_originalScale * 0.9f, 0.1f).SetEase(Ease.OutQuad);
            SO_Manager.Load_SO<GridSignals>().OnGridElementChanged?.Invoke(this);
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            _upTween = transform.DOScale(_originalScale, 0.25f).SetEase(Ease.OutQuad);
        }
    }
}