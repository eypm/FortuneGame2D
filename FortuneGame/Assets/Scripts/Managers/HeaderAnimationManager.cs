using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class HeaderAnimationManager : MonoBehaviour
{
    public static bool isAnimateWaiting;

    [SerializeField] private GameObject animationPrefab;

    [Space] [Header("Targets")] [SerializeField]
    private Sprite coinSprite;

    [SerializeField] private Sprite heartSprite;
    [SerializeField] private Sprite keySprite;
    [SerializeField] private Sprite balloonSprite;
    [SerializeField] private Sprite teamBallSprite;
    [SerializeField] private Sprite bullseyeSprite;
    [SerializeField] private Sprite discoSprite;

    [Space] [Header("Targets")] [SerializeField]
    private Transform coinTarget;

    [SerializeField] private Transform heartTarget;
    [SerializeField] private Transform keyTarget;
    [SerializeField] private Transform inventoryTarget;
    [SerializeField] private Transform screenTopCenterPosition;
    [SerializeField] private Transform teamsCoinTarget;
    [SerializeField] private Transform liveBankHeartTarget;
    [SerializeField] private Transform balloonEventButtonTarget;
    [SerializeField] private Transform teamEventButtonTarget;
    [SerializeField] private Transform bullseyeButtonTarget;
    [SerializeField] private Transform discoButtonTarget;

    [Space] [Header("Play Button Anim")] [SerializeField]
    private Transform playButton;

    [SerializeField] private Transform playButtonText;

    [SerializeField] [Range(0.6f, 2f)] float maxAnimDuration = 0.6f;

    [SerializeField] Ease easeType = Ease.InOutSine;
    // [SerializeField] float spread = 50f;

    public void Animate(HeaderAnimationPath animationPath, bool isReverse, int order, UnityAction onCompleted, UnityAction onStart = null, bool activateItemOnStart = false)
    {
        Vector3 sourcePosition = animationPath.sourcePosition;
        HeaderAnimationTargetType targetType = animationPath.targetType;
        Sprite sprite = animationPath.sprite != null ? animationPath.sprite : GetTargetSprite(animationPath.targetType);
        int amount = animationPath.amount;

        RectTransform targetTransform = animationPath.targetTransform != null ? animationPath.targetTransform : GetTargetTransform(animationPath.targetType);
        Vector3 targetPosition = targetTransform.position;

        for (int i = 0; i < amount; i++)
        {
          
            GameObject pool = Instantiate(animationPrefab, targetTransform, false); 
            HeaderAnimationItem headerAnimationItem = pool.GetComponent<HeaderAnimationItem>();
            headerAnimationItem.image.sprite = sprite;
            if (!activateItemOnStart)
                pool.SetActive(true);
            headerAnimationItem.rectTransform.position = (isReverse ? targetPosition : sourcePosition);
            if (animationPath.sourceSizeDelta != Vector2.zero)
            {
                headerAnimationItem.rectTransform.sizeDelta = animationPath.sourceSizeDelta;
            }

            if (animationPath.sortingOrder > 0)
            {
                headerAnimationItem.canvas.sortingOrder = animationPath.sortingOrder;
            }

            var i1 = i;
            App.instance.WaitForSeconds(0.5f + (order * 0.3f), () =>
            {
                pool.SetActive(true);
                onStart?.Invoke();
                float duration = maxAnimDuration; 

                if (targetType is HeaderAnimationTargetType.Heart)
                {
                    headerAnimationItem.rectTransform.DOSizeDelta(headerAnimationItem.rectTransform.sizeDelta * 1.5f, duration / 3f).OnComplete(() =>
                    {
                        headerAnimationItem.rectTransform.DOSizeDelta(targetTransform.sizeDelta, duration - (duration / 3f));
                    });
                }
                else if (targetType is HeaderAnimationTargetType.Coin or HeaderAnimationTargetType.TeamsCoin or HeaderAnimationTargetType.Key or HeaderAnimationTargetType.LiveBankHeart)
                {
                    headerAnimationItem.rectTransform.DOSizeDelta(targetTransform.sizeDelta, duration);
                }

                if (targetType == HeaderAnimationTargetType.Inventory)
                {
                    headerAnimationItem.rectTransform.DOScale(1.2f, 0.3f);
                    headerAnimationItem.rectTransform.DOMoveY(headerAnimationItem.rectTransform.position.y + 40, 0.2f).OnComplete(() =>
                    {
                        headerAnimationItem.rectTransform.DOScale(0.8f, duration);
                        headerAnimationItem.rectTransform.DOLocalMove(Vector3.zero, duration / 2)
                            .SetEase(Ease.InOutSine).OnComplete(() =>
                            {
                                headerAnimationItem.ShowParticle();
                                if (i1 == amount - 1)
                                {
                                    targetTransform.DOKill(true);
                                    playButton.DOScale(new Vector3(.95f, .9f, 1f), 0.1f).SetEase(Ease.InSine).OnComplete(() => playButton.DOScale(1f, 0.07f).SetEase(Ease.InSine));
                                    playButtonText.DOScale(0.9f, 0.095f).SetEase(Ease.InSine).OnComplete(() => playButtonText.DOScale(1f, 0.075f).SetEase(Ease.InSine));
                                    AudioManager.GetInstance().PlaySound(animationPath.audio);

                                    onCompleted?.Invoke();
                                }
                            });
                    });
                }
                else
                {
                    headerAnimationItem.rectTransform.DOLocalMove(Vector3.zero, duration).SetEase(easeType)
                        .OnComplete(() =>
                        {
                            headerAnimationItem.ShowParticle();
                            if (i1 == amount - 1)
                            {
                                targetTransform.DOKill(true);
                                if (targetType is not HeaderAnimationTargetType.TeamsCoin and HeaderAnimationTargetType.LiveBankHeart)
                                    targetTransform.DOShakeScale(0.3f, 0.5f, vibrato: 3);
                                AudioManager.GetInstance().PlaySound(animationPath.audio);
                                onCompleted?.Invoke();
                            }
                        });
                }
            });
        }
    }

    public void Animate(HeaderAnimationPath[] animationPaths, Action<HeaderAnimationTargetType> animationCompletion, Action completion)
    {
        for (var i = 0; i < animationPaths.Length; i++)
        {
            HeaderAnimationPath animationPath = animationPaths[i];
            var i1 = i;
            if (animationPath.sourceTransform != null)
            {
                Animate(animationPath, false, i,
                    () =>
                    {
                        animationCompletion?.Invoke(animationPath.targetType);
                        if (i1 == animationPaths.Length - 1)
                        {
                            completion?.Invoke();
                        }
                    });
            }
            else
            {
                Animate(animationPath, false, i, () =>
                {
                    animationCompletion?.Invoke(animationPath.targetType);
                    if (i1 == animationPaths.Length - 1)
                    {
                        completion?.Invoke();
                    }
                });
            }
        }
    }

 
    private RectTransform GetTargetTransform(HeaderAnimationTargetType targetType)
    {
        return targetType switch
        {
            HeaderAnimationTargetType.Coin => (RectTransform) coinTarget,
            HeaderAnimationTargetType.Heart => (RectTransform) heartTarget,
            HeaderAnimationTargetType.Inventory => (RectTransform) inventoryTarget,
            HeaderAnimationTargetType.Key => (RectTransform) keyTarget,
            HeaderAnimationTargetType.TeamsCoin => (RectTransform) teamsCoinTarget,
            HeaderAnimationTargetType.LiveBankHeart => (RectTransform) liveBankHeartTarget,
            HeaderAnimationTargetType.BalloonEvent => (RectTransform) balloonEventButtonTarget,
            HeaderAnimationTargetType.TeamEventBall => (RectTransform) teamEventButtonTarget,
            HeaderAnimationTargetType.Bullseye => (RectTransform) bullseyeButtonTarget,
            HeaderAnimationTargetType.DiscoEventBall => (RectTransform) discoButtonTarget,

            _ => null
        };
    }

    private Sprite GetTargetSprite(HeaderAnimationTargetType targetType)
    {
        switch (targetType)
        {
            case HeaderAnimationTargetType.Coin: return coinSprite;
            case HeaderAnimationTargetType.Heart: return heartSprite;
            case HeaderAnimationTargetType.Key: return keySprite;
            case HeaderAnimationTargetType.TeamsCoin: return coinSprite;
            case HeaderAnimationTargetType.LiveBankHeart: return heartSprite;
            case HeaderAnimationTargetType.BalloonEvent: return balloonSprite;
            case HeaderAnimationTargetType.TeamEventBall : return teamBallSprite;
            case HeaderAnimationTargetType.DiscoEventBall : return discoSprite;
            case HeaderAnimationTargetType.Bullseye : return bullseyeSprite;

            default: return null;
        }
    }
}

public enum HeaderAnimationTargetType
{
    None = -1,
    Coin = 0,
    Heart = 1,
    Key = 2,
    Inventory = 3,
    TeamsCoin = 4,
    LiveBankHeart = 5,
    BalloonEvent = 6,
    TeamEventBall = 7,
    Bullseye = 8,
    DiscoEventBall = 9,
    Custom =10,
}

public class HeaderAnimationPath
{
    public HeaderAnimationTargetType targetType;
    public Transform sourceTransform;
    public RectTransform targetTransform;
    public Sprite sprite;
    public Vector3 sourcePosition;
    public int amount;
    public Vector2 sourceSizeDelta = Vector2.zero;
    public Audio audio;
    public int sortingOrder;

    public HeaderAnimationPath(HeaderAnimationTargetType targetType, Transform sourceTransform, int amount = 10, Sprite sprite = null,Audio audio = null)
    {
        this.targetType = targetType;
        this.sourceTransform = sourceTransform;
        this.sourcePosition = sourceTransform.position;

        if (sourceTransform is RectTransform rectTransform)
        {
            sourceSizeDelta = rectTransform.sizeDelta;
        }

        if (targetType is HeaderAnimationTargetType.Inventory or HeaderAnimationTargetType.Heart)
        {
            this.sprite = sprite;
        }
        else
        {
            this.sprite = null;
        }

        this.amount = amount;

        if (audio != null)
            this.audio = audio;
        else
        {
            this.audio = AudioManager.Sounds.headerAnimationCoin;
        }
    }
    
    public HeaderAnimationPath(RectTransform targetTransform, Transform sourceTransform,int sortingOrder = 0 , int amount = 10, Sprite sprite = null,Audio audio = null)
    {
        this.sourceTransform = sourceTransform;
        this.targetTransform = targetTransform;
        this.sprite = sprite;
        this.sortingOrder = sortingOrder;
        sourcePosition = sourceTransform.position;

        if (sourceTransform is RectTransform rectTransform)
        {
            sourceSizeDelta = rectTransform.sizeDelta;
        }
        
        
        this.amount = amount;

        if (audio != null)
            this.audio = audio;
   
    }

    public HeaderAnimationPath(HeaderAnimationTargetType targetType, Vector3 sourcePosition, Vector2? sourceSizeDelta = null, int amount = 10, Sprite sprite = null)
    {
        this.targetType = targetType;
        this.sourceTransform = null;
        this.sourcePosition = sourcePosition;
        this.sourceSizeDelta = sourceSizeDelta ?? Vector2.zero;

        if (targetType is HeaderAnimationTargetType.Inventory or HeaderAnimationTargetType.Heart)
        {
            this.sprite = sprite;
        }
        else
        {
            this.sprite = null;
        }

        this.amount = amount;
    }
}

[Serializable]
public struct HeaderAnimationTargetInfo
{
    public HeaderAnimationTargetType type;
    public Sprite sprite;
    public RectTransform targetTransform;
}