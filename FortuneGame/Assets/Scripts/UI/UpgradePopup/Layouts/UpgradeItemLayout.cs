using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItemLayout : MonoBehaviour
{
    public Image imgBg;
    public Button upgradeButton;
    public TextMeshProUGUI txtUpgradeCost;
    public TextMeshProUGUI txtLevel;
    public TextMeshProUGUI txtPoints;
    public Slider pointSlider;
    private int cardLevel;
    private Card mCard;

    void Start()
    {
        upgradeButton.onClick.AddListener(Upgrade);
        pointSlider.minValue = 0;
        pointSlider.interactable = false;
    }

    public void Fill(Card card)
    {
        mCard = card;
        UpdateContent();
    }

    private void UpdateContent()
    {
        cardLevel = App.user.GetCardLevel(mCard.cardType);
        int maxPoint = mCard.GetCardPoint(cardLevel);
        int currentPoint = App.user.GetCardPoint(mCard.cardType);
        txtPoints.text = currentPoint + "/" + maxPoint;
        pointSlider.maxValue = maxPoint;
        pointSlider.value = currentPoint;
        imgBg.sprite = mCard.cardType.GetSprite();
        txtUpgradeCost.text = mCard.GetUpgradeCost(cardLevel).ToAbbreviatedString();
        txtLevel.text = "Lvl " + cardLevel;
    }

    private bool IsUpgraded()
    {
        if (mCard.GetCardPoint(cardLevel) > App.user.GetCardPoint(mCard.cardType))
            return false;
        if (mCard.IsMaxed(cardLevel))
            return false;
        if (mCard.GetUpgradeCost(cardLevel) > App.user.money)
            return false;
        return true;
    }

    private void Upgrade()
    {
        if (!IsUpgraded())
        {
            Snackbar.Show("Cannot be upgraded");
            return;
        }

        App.user.UpdateMoney(-mCard.GetUpgradeCost(App.user.GetCardLevel(mCard.cardType)));
        App.user.IncrementCardPoint(mCard.cardType, -mCard.GetCardPoint(App.user.GetCardLevel(mCard.cardType)));
        App.user.IncrementCardLevel(mCard.cardType);

        UpdateContent();
    }
}