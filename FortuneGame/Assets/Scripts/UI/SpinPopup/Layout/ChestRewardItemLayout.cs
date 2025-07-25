using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestRewardItemLayout : MonoBehaviour
{
    public Transform trThis;
    public Image imgReward;
    public TextMeshProUGUI txtAmount;
    public GameObject goThis;

    public void Reset()
    {
        trThis.localScale = Vector3.zero;
        goThis.SetActive(false);
    }

    public void Fill(ChestReward reward)
    {
        goThis.SetActive(true);
        imgReward.sprite = reward.cardType.GetSprite();
        txtAmount.text = reward.point.ToString();
    }

    public void PlayAnimation(float delay = 0)
    {
        trThis.DOScale(Vector3.one, 1f).Delay();
    }
}