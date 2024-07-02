using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CollectNumbers
{
    public class NumberBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public int index;
        public bool isActive = true;
        public SelectedNumber selectedNumber;
        [SerializeField] private TextMeshProUGUI numberText;
        [SerializeField] private Image numberImage;
        [SerializeField] private GameObject effectPrefab;

        private int _clickCount = 0;
        private int _movementRight = 0;
        private Vector3 _originalScale;
        private Tween downTween, upTween;
        public void Initialize(string number, Color color, SelectedNumber selectedNumber, int movementRight)
        {
            _movementRight = movementRight;
            if(_clickCount > _movementRight) return;
            this.selectedNumber = selectedNumber;
            numberText.text = number;
            numberImage.color = color;
            isActive = true;
            _originalScale = transform.localScale;
        }

        public void ResetClickCount()
        {
            _clickCount = 0;
        }

        public void Explode(bool activeness)
        {
            effectPrefab.SetActive(activeness);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _clickCount++;
            if(!GameManager.Instance.gameIsActive || !isActive || _clickCount > _movementRight)
            {
                if (_clickCount > _movementRight)
                {
                    numberImage.color = Color.black;
                    numberImage = null;
                    numberText.text = "";
                    selectedNumber = SelectedNumber.Null;
                }
                downTween.Kill();
                upTween.Kill();
                return;
            }
            downTween = transform.DOScale(_originalScale * 0.95f, 0.1f).SetEase(Ease.OutQuad);
            SO_Manager.Load_SO<GridSignals>().OnGridElementChanged?.Invoke(this);
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            upTween = transform.DOScale(_originalScale, 0.1f).SetEase(Ease.OutQuad);
        }
    }
}