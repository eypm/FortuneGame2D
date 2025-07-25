using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button btnSpinPopup;
    public Button btnUpgradePopup;
    public Button btnSound;
    public GameObject goSoundCross;
    public TextMeshProUGUI txtGold;
    public TextMeshProUGUI txtMoney;
    public static MainMenuManager instance;

    void Start()
    {
        instance = this;
        btnSound.onClick.AddListener(SoundButton);
        btnSpinPopup.onClick.AddListener(() => UIManager.instance.InstantiatePopup(PopupType.SpinPopup));
        btnUpgradePopup.onClick.AddListener(() => UIManager.instance.InstantiatePopup(PopupType.UpgradePopup));
        UpdateCoinsValue();
        App.user.OnCurrencyAmountChanged.AddListener(UpdateCoinsValue);
        AudioManager.GetInstance().PlaySound(AudioManager.Sounds.gameMusic);
        goSoundCross.SetActive(!PreferenceHelper.IsSoundEnabled());
    }

    private void SoundButton()
    {
        if (PreferenceHelper.IsSoundEnabled())
        {
            goSoundCross.SetActive(true);
            PreferenceHelper.SetSoundEnabled(false);
            AudioManager.GetInstance().StopIfPlaying(AudioManager.Sounds.gameMusic);
        }
        else
        {
            goSoundCross.SetActive(false);
            PreferenceHelper.SetSoundEnabled(true);
            AudioManager.GetInstance().PlaySound(AudioManager.Sounds.gameMusic);
        }
    }

    private void UpdateCoinsValue()
    {
        txtGold.text = App.user.gold.ToAbbreviatedString();
        txtMoney.text = App.user.money.ToAbbreviatedString();
    }
}