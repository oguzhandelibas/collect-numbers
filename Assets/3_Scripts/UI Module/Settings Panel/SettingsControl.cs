using DG.Tweening;
using MoreMountains.NiceVibrations;
using UnityEngine;
using UnityEngine.UI;

public class SettingsControl : MonoBehaviour
{
    [SerializeField] private RectTransform settingsHolder;
    [SerializeField] Image hapticButtonImage;
    private bool _isHapticActive = true;

    private bool settingsToggle = false;
    private Vector2 settingsSizeDelta;
    
    private void Start()
    {
        settingsToggle = false;
        settingsSizeDelta = settingsHolder.sizeDelta;
        ToggleSettingsButtonClick();
    }
    
    public void ToggleSettingsButtonClick()
    {
        var sizeDelta = settingsSizeDelta;
        
        if (settingsToggle) sizeDelta = new Vector2(sizeDelta.x, sizeDelta.y);
        else sizeDelta = new Vector2(sizeDelta.x, sizeDelta.y / 3f);
        settingsToggle = !settingsToggle;
        
        settingsHolder.DOSizeDelta(sizeDelta, .25f);
    }
    
    public void SetHapticActiveness()
    {
        _isHapticActive = !_isHapticActive;
        if (_isHapticActive) hapticButtonImage.color = new Color(0.2941177f, 0.3764706f, 0.4745098f);
        else hapticButtonImage.color = new Color(0.70f, 0.70f, 0.70f);
        MMVibrationManager.SetHapticsActive(_isHapticActive);
    }
}
