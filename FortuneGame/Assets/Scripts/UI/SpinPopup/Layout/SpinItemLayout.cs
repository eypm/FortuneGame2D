using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpinItemLayout : MonoBehaviour
{
    public RectTransform trLayout;
    public Image imgReward;
    public TextMeshProUGUI txtRewardAmount;
    public RectTransform rtBgImg;
    [NonSerialized] public Reward mReward;

    public void SetContent(Reward reward)
    {
        mReward = reward;
        RewardType rewardType = mReward.rewardType;
        if (rewardType is RewardType.Coin or RewardType.Money)
            txtRewardAmount.text = mReward.rewardAmount.ToAbbreviatedString();
        else
            txtRewardAmount.text = "";
        imgReward.sprite = rewardType.GetSprite();
        rtBgImg.sizeDelta = rewardType.GetSpinItemSpriteSize();
    }
}