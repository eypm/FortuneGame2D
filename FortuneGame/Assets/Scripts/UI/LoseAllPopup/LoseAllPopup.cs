using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoseAllPopup : UIPopup
{
    public GameObject goBtnReviveAd;
    public TextMeshProUGUI txtReviveAmount;
    public Button btnRevive;
    public Button btnReviveAd;
    public Button btnGiveUp;
    private int reviveAmount;

    void Start()
    {
        AudioManager.GetInstance().PlaySound(AudioManager.Sounds.failed);
        btnGiveUp.onClick.AddListener(GiveUp);
        btnRevive.onClick.AddListener(Revive);
        if (App.user.loseAllCount > 0)
            goBtnReviveAd.SetActive(false);
        else
            btnReviveAd.onClick.AddListener(ReviveAd);
        reviveAmount = Config.reviveAmounData.reviveDatas[App.user.loseAllCount];
        txtReviveAmount.text = reviveAmount.ToAbbreviatedString();
    }

    private void Revive()
    {
        if (reviveAmount > App.user.gold)
        {
            Snackbar.Show("Insufficient gold");
            return;
        }

        App.user.UpdateGold(-reviveAmount);
        App.user.UpdateLoseAllCount(1);
        ClosePopup();
    }

    private void ReviveAd()
    {
        App.user.UpdateLoseAllCount(1);
        ClosePopup();
    }

    private void GiveUp()
    {
        UIManager.instance.InstantiatePopup(PopupType.InfoPopup,onAction);
    }
}