using UnityEngine;

[CreateAssetMenu(fileName = "Audio Settings", menuName = "Settings/Audio Settings")]
public class AudioSettings : ScriptableObject
{
    public Sounds sounds;
}
[System.Serializable]
public class Sounds
{
    public Audio buttonClick;
    public Audio popupOpen;
    public Audio buttonGeneric;
    public Audio spin;
    public Audio gameMusic;
    public Audio rewardCollecting;
    public Audio headerAnimationCoin;
    public Audio spinRewardOpening;
    public Audio spinReward;
    public Audio failed;
}
