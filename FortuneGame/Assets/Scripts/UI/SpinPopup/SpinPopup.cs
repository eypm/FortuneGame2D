using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpinPopup : UIPopup
{
    public Ease ease = Ease.OutSine;
    public Button btnClose;
    public Button btnSpin;
    public Button btnRewardCollect;
    public RectTransform rtPopupInside;
    public RectTransform rtHeader;
    public RectTransform trLayout;
    public RectTransform trRewardPanelFlash;
    public RectTransform trRewardPanelReward;

    public Image imgRewardPanelBg;
    public Image imgRewardPanelReward;
    public Image imgRewardPanelFlash;
    public Image imgBg;
    public Image imgIndicator;

    public GameObject goRewardPanel;
    public GameObject goEarnRewardPanel;
    public GameObject goRewardCollectBtn;

    public TextMeshProUGUI txtTitle;
    public TextMeshProUGUI txtRewardPanelRewardAmount;

    public List<SpinItemLayout> spinItemLayouts;
    public List<ChestRewardItemLayout> chestRewardItemLayouts;
    public List<EarnChestRewardItemLayout> earnChestRewardItemLayouts;
    public List<EarnCurrencyRewardItemLayout> earnCurrencyRewardItemLayouts;

    private ZoneData[] zoneDatas;
    private ZoneData currentZoneData;
    private ZoneType currentZoneType = ZoneType.None;

    void Start()
    {
        zoneDatas = Config.spinDatabase.zoneDatas;
        btnClose.onClick.AddListener(CloseButton);
        btnSpin.onClick.AddListener(SpinButton);
        btnRewardCollect.onClick.AddListener(CollectReward);
        PlayShowAnimation();
        FillContent();
    }

    private void PlayShowAnimation()
    {
        rtPopupInside.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        rtHeader.localScale = new Vector3(0.8f, 0.8f, 0.8f);

        var seq1 = DOTween.Sequence();
        seq1.Append(rtPopupInside.DOScale(1.1f, 0.22f).SetEase(Ease.OutQuad));
        seq1.Append(rtPopupInside.DOScale(0.95f, 0.073f).SetEase(Ease.InSine));
        seq1.Append(rtPopupInside.DOScale(1, 0.073f).SetEase(Ease.InSine));

        var seq2 = DOTween.Sequence();
        seq2.SetDelay(0.09f);
        seq2.Append(rtHeader.DOScale(1.1f, 0.22f).SetEase(Ease.OutQuad));
        seq2.Append(rtHeader.DOScale(0.95f, 0.073f).SetEase(Ease.InSine));
        seq2.Append(rtHeader.DOScale(1, 0.073f).SetEase(Ease.InSine));
    }

    private void FillContent()
    {
        currentZoneData = zoneDatas.First(x => x.zoneLevel == App.user.zoneLevel);
        Reward[] rewards = currentZoneData.rewards;
        for (int i = 0; i < spinItemLayouts.Count; i++)
        {
            if (i < rewards.Length)
            {
                Reward reward = rewards[i];
                spinItemLayouts[i].SetContent(reward);
            }
        }

        if (currentZoneType != currentZoneData.zoneType)
        {
            currentZoneType = currentZoneData.zoneType;
            txtTitle.text = currentZoneType.GetZoneTitle();
            imgBg.sprite = currentZoneType.GetZoneBgSprite();
            imgIndicator.sprite = currentZoneType.GetZoneIndicatorSprite();
        }
    }


    private void UpdateContent()
    {
        FillContent();
        trLayout.localRotation = Quaternion.Euler(0, 0, 0);
        btnSpin.interactable = true;
        btnRewardCollect.interactable = true;
        btnClose.interactable = true;
    }

    private void CollectReward()
    {
        AudioManager.GetInstance().PlaySound(AudioManager.Sounds.rewardCollecting);
        for (int i = 0; i < earnChestRewardItemLayouts.Count; i++)
        {
            EarnChestRewardItemLayout layout = earnChestRewardItemLayouts[i];
            if (layout.cardType != CardType.None)
            {
                App.user.IncrementCardPoint(layout.cardType, layout.currentAmount);
            }
        }

        for (int i = 0; i < earnCurrencyRewardItemLayouts.Count; i++)
        {
            EarnCurrencyRewardItemLayout layout = earnCurrencyRewardItemLayouts[i];
            if (layout.rewardType == RewardType.Money)
            {
                App.user.UpdateMoney(layout.currentAmount);
            }
            else if (layout.rewardType == RewardType.Coin)
            {
                App.user.UpdateGold(layout.currentAmount);
            }
        }

        App.user.IncrementZoneLevel(-App.user.zoneLevel + 1);
        CloseAllPopup();
    }

    private void SpinButton()
    {
        StartSpin();
        AudioManager.GetInstance().PlaySound(AudioManager.Sounds.spin);
        btnSpin.interactable = false;
        btnRewardCollect.interactable = false;
        btnClose.interactable = false;
    }


    private void StartSpin()
    {
        float firstValue = UnityEngine.Random.Range(0, 360f);
        float finalValue = GetSpinFinalValue(firstValue);
        var seq = DOTween.Sequence();
        seq.Append(trLayout.DOLocalRotate(new Vector3(0, 0, -(1800 - firstValue)), 5, RotateMode.FastBeyond360)
            .SetRelative(true).SetEase(Ease.OutQuint));
        seq.Append(trLayout.DOLocalRotate(new Vector3(0, 0, -finalValue), 1, RotateMode.FastBeyond360)
            .SetRelative(true).SetEase(Ease.OutQuint).OnComplete(() => { SpinCompleted(firstValue); }));
    }

    private float GetSpinFinalValue(float firstValue)
    {
        float[] targetAngles = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f, 360f };
        float closest = targetAngles[0];
        float minDiff = Mathf.Abs(firstValue - closest);

        foreach (float angle in targetAngles)
        {
            float diff = Mathf.Abs(firstValue - angle);
            if (diff < minDiff)
            {
                minDiff = diff;
                closest = angle;
            }
        }

        return firstValue - closest;
    }

    private bool isCollectable = false;

    private int GetSpinIndexFromAngle(float angle)
    {
        angle %= 360;
        if (angle < 0) angle += 360;
        int index = Mathf.FloorToInt((angle + 22.5f) / 45f) % 8;

        switch (index)
        {
            case 0: return 4;
            case 1: return 3;
            case 2: return 2;
            case 3: return 1;
            case 4: return 0;
            case 5: return 7;
            case 6: return 6;
            case 7: return 5;
            default: return -1;
        }
    }

    private void SpinCompleted(float angle)
    {
        int index = GetSpinIndexFromAngle(angle);
        var layout = spinItemLayouts[index];
        Reward reward = layout.mReward;
        if (reward.rewardType == RewardType.Bomb)
        {
            UIManager.instance.InstantiatePopup(PopupType.LoseAllPopup);
            App.user.IncrementZoneLevel();
            UpdateContent();
        }
        else
        {
            AudioManager.GetInstance().PlaySound(AudioManager.Sounds.spinReward);
            PlayRewardPanelAnim(reward, (() =>
            {
                if (!isCollectable)
                {
                    goRewardCollectBtn.SetActive(true);
                    goEarnRewardPanel.SetActive(true);
                }

                isCollectable = true;
                AudioManager.GetInstance().PlaySound(AudioManager.Sounds.spinRewardOpening);
                if (!reward.rewardType.IsChestReward())
                    UpdateEarnRewardPanel(reward);

                App.user.IncrementZoneLevel();
                UpdateContent();
                ResetRewardPanel();
            }));
        }
    }

    private void UpdateEarnRewardPanel(ChestReward[] rewards)
    {
        for (int i = 0; i < rewards.Length; i++)
        {
            ChestReward reward = rewards[i];
            EarnChestRewardItemLayout layout =
                earnChestRewardItemLayouts.FirstOrDefault(x => x.cardType == reward.cardType);
            if (layout != null)
            {
                layout.IncrementAmount(reward.point);
            }
            else
            {
                layout = earnChestRewardItemLayouts.FirstOrDefault(x => x.cardType == CardType.None);
                layout.Fill(reward);
            }
        }
    }


    private void UpdateEarnRewardPanel(Reward reward)
    {
        EarnCurrencyRewardItemLayout layout =
            earnCurrencyRewardItemLayouts.FirstOrDefault(x => x.rewardType == reward.rewardType);
        if (layout != null)
        {
            layout.IncrementAmount(reward.rewardAmount);
        }
        else
        {
            layout = earnCurrencyRewardItemLayouts.FirstOrDefault(x => x.rewardType == RewardType.None);
            layout.Fill(reward);
        }
    }

    private void PlayRewardPanelAnim(Reward reward, Action onComplete)
    {
        RewardType rewardType = reward.rewardType;
        goRewardPanel.SetActive(true);
        imgRewardPanelReward.sprite = rewardType.GetSprite();
        txtRewardPanelRewardAmount.text = rewardType.IsChestReward() ? "" : reward.rewardAmount.ToAbbreviatedString();
        imgRewardPanelBg.DOColor(new Color(0, 0, 0, 1), 1f);
        trRewardPanelReward.sizeDelta = rewardType.GetSpinRewardPanelSpriteSize();
        trRewardPanelReward.DOScale(Vector3.one, 1.3f);
        imgRewardPanelFlash.DOColor(new Color(1, 0.85f, 0.26f, 1f), 1.3f)
            .OnComplete(() =>
            {
                if (rewardType.IsChestReward())
                {
                    trRewardPanelReward.DOLocalMove(new Vector3(0, -100f, 0f), 1f);
                    ChestLevel level = Config.chestDatabase.GetChestLevel(rewardType.GetChestType());
                    ChestReward[] rewards = level.rewards;
                    UpdateEarnRewardPanel(rewards);

                    for (int i = 0; i < rewards.Length; i++)
                    {
                        ChestRewardItemLayout layout = chestRewardItemLayouts[i];
                        layout.Fill(rewards[i]);
                        float delay = i / 2f;
                        layout.PlayAnimation(delay);
                    }
                }

                trRewardPanelFlash.DOLocalRotate(new Vector3(0, 0, -360), 4f).SetRelative(true).SetEase(Ease.Linear)
                    .OnComplete(onComplete.Invoke);
            });
    }

    private void ResetRewardPanel()
    {
        imgRewardPanelBg.color = new Color(0, 0, 0f, 0);
        imgRewardPanelFlash.color = new Color(1, 0.85f, 0.26f, 0);
        trRewardPanelReward.localScale = Vector3.zero;
        trRewardPanelReward.localPosition = Vector3.zero;
        goRewardPanel.SetActive(false);
        for (int i = 0; i < chestRewardItemLayouts.Count; i++)
        {
            chestRewardItemLayouts[i].Reset();
        }
    }

    private void CloseButton()
    {
        if(isCollectable)
            UIManager.instance.InstantiatePopup(PopupType.InfoPopup);else  CloseAllPopup();
    }
}