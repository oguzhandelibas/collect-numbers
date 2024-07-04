using AYellowpaper.SerializedCollections;
using UnityEngine;

public enum AudioType
{
    Click,
    Match,
    Movement,
    Combo,
    Success,
    Fail
}

[CreateAssetMenu(fileName = "AudioData", menuName = "ScriptableObjects/AudioData", order = 1)]
public class AudioData : ScriptableObject
{
    [SerializedDictionary("Audio Type", "Audio Clip")]
    public SerializedDictionary<AudioType, AudioClip> AudioEffects;
}
