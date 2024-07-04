using CollectNumbers;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : AbstractSingleton<AudioManager>
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectSource;
    [SerializeField] private Image musicButtonImage;
    [SerializeField] private Image soundButtonImage;
    
    private AudioData _audioData;
    
    private bool isSoundActive = true;
    private bool isMusicActive = false;

    private void Start()
    {
        _audioData = SO_Manager.Load_SO<AudioData>();
        isSoundActive = false;
        isMusicActive = true;
        _SetMusicActiveness();
        _SetSoundActiveness();
    }
    
    public void _SetMusicActiveness()
    {
        isMusicActive = !isMusicActive;
        if (isMusicActive) musicButtonImage.color = new Color(0.2941177f, 0.3764706f, 0.4745098f);
        else musicButtonImage.color = new Color(0.70f, 0.70f, 0.70f);
        if (isMusicActive)
        {
            musicSource.volume = 0.05f;
        }
        else
        {
            musicSource.volume = 0.0f;
        }
    }

    public void _SetSoundActiveness()
    {
        isSoundActive = !isSoundActive;
        if (isSoundActive) soundButtonImage.color = new Color(0.2941177f, 0.3764706f, 0.4745098f);
        else soundButtonImage.color = new Color(0.70f, 0.70f, 0.70f);
        if (isSoundActive)
        {
            effectSource.volume = 0.5f;
        }
        else
        {
            effectSource.volume = 0.0f;
        }
    }

    public void PlayAudioEffect(AudioType audioType)
    {
        if (effectSource.isPlaying)
        {
            effectSource.Stop();
            effectSource.clip = null;
        }
        
        effectSource.clip = _audioData.AudioEffects[audioType];
        effectSource.Play();
    }
}
