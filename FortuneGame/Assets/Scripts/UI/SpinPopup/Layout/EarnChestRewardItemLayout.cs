using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EarnChestRewardItemLayout : MonoBehaviour
{
    public Image imgReward;
    public TextMeshProUGUI txtAmount;
    public GameObject goThis;

    public int currentAmount = 0;
    public CardType cardType;

    public void Fill(ChestReward reward)
    {
        cardType = reward.cardType;
        goThis.SetActive(true);
        imgReward.sprite = reward.cardType.GetSprite();
        currentAmount += reward.point;
        txtAmount.text = currentAmount.ToString();
    }

    public void IncrementAmount(int amount)
    {
        currentAmount += amount;
        txtAmount.text = currentAmount.ToString();
    }
}