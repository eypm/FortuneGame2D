using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EarnCurrencyRewardItemLayout : MonoBehaviour
{
    public Image imgReward;
    public TextMeshProUGUI txtAmount;
    public GameObject goThis;
    public RewardType rewardType;
    public int currentAmount = 0;

    public void Fill(Reward reward)
    {
        rewardType = reward.rewardType;
        goThis.SetActive(true);
        imgReward.sprite = reward.rewardType.GetSprite();
        currentAmount += reward.rewardAmount;
        txtAmount.text = currentAmount.ToAbbreviatedString();
    }

    public void IncrementAmount(int amount)
    {
        currentAmount += amount;
        txtAmount.text = currentAmount.ToAbbreviatedString();
    }
}