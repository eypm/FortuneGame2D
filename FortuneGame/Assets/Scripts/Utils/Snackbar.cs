using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class Snackbar : MonoBehaviour
{
    public const float DURATION_SHORT = 1f;
    public const float DURATION_NORMAL = 3f;
    public const float DURATION_LONG = 5f;
    public const float DURATION_INFINITE = float.MaxValue;
    private const float ANIM_DURATION = 0.22f;

    [SerializeField] private GameObject goContent;
    [SerializeField] private RectTransform rtContent;
    [SerializeField] private Button btnAction;
    [SerializeField] private TextMeshProUGUI txtMessage;
    private float mDuration = DURATION_SHORT;
    private float mHiddenTranslationY;
    private string mMessage;

    private Canvas currentCanvas;

    private Vector3 mStartPosition;

    private static Snackbar Create()
    {
        GameObject go = Instantiate(Resources.Load("Prefabs/Widget/Snackbar")) as GameObject;
        Snackbar newInstance = go.GetComponent<Snackbar>();
        go.AddComponent<Canvas>();
        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999999 + (App.instance.snackbars?.Count ?? 0);
        newInstance.currentCanvas = canvas;
        go.AddComponent<CanvasScaler>();
        CanvasScaler canvasScaler = go.GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(800, 600);
        return newInstance;
    }

    public static void Show(string message, float duration = DURATION_SHORT)
    {
        if (App.instance.snackbars == null)
        {
            App.instance.snackbars = new List<Snackbar>();
        }

        for (var i = 0; i < App.instance.snackbars.Count; i++)
        {
            if (App.instance.snackbars[i] != null)
                App.instance.snackbars[i].Destroy();
        }

        Snackbar currentSnackbar = App.instance.snackbars.FirstOrDefault(x => x != null && x.mMessage.Equals(message));
        if (currentSnackbar == null)
        {
            currentSnackbar = Create();
            currentSnackbar.SetMessage(message);
            currentSnackbar.SetDuration(duration);
            currentSnackbar.PlayShowAnimation(false);
            App.instance.snackbars.Add(currentSnackbar);
        }
    }

    void Awake()
    {
        btnAction.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        transform.SetAsLastSibling();
    }

    private Snackbar SetMessage(string message)
    {
        mMessage = message;
        txtMessage.text = mMessage;
        return this;
    }

    public Snackbar SetDuration(float duration)
    {
        mDuration = duration;
        return this;
    }

    public Snackbar SetAction(string text, UnityAction action)
    {
        btnAction.gameObject.SetActive(true);
        btnAction.GetComponentInChildren<TMP_Text>().text = text;
        btnAction.onClick.AddListener(delegate
        {
            Hide();
            action.Invoke();
        });
        return this;
    }

    public void Hide()
    {
        transform.DOMoveX(15, ANIM_DURATION);
        Invoke(nameof(Destroy), ANIM_DURATION);
    }

    public void Destroy()
    {
        App.instance.snackbars?.Remove(this);
        App.instance.snackbars?.RemoveAll(x => x == null);
        Destroy(gameObject);
    }

    private void BringToFront()
    {
        currentCanvas.sortingOrder = 9999 + (App.instance.snackbars?.Count ?? 0) + 1;
    }

    private void PlayShowAnimation(bool isRestart)
    {
        if (isRestart)
        {
            var transformPosition = rtContent.position;
            transformPosition.y = mHiddenTranslationY + 50f;
            rtContent.position = transformPosition;
        }
        else
        {
            mStartPosition = rtContent.position;
            float height = 5;
            float visiblex = mStartPosition.x;
            mHiddenTranslationY = visiblex + height;
        }

        Color color = txtMessage.color;
        DOTween.To(() => 3.5f, x => color.a = x, 0f, mDuration).OnUpdate(() => { txtMessage.color = color; });

        rtContent.DOAnchorPosY(rtContent.anchoredPosition.y + 60, mDuration)
            .OnComplete(() => txtMessage.DOFade(0, mDuration));

        CancelInvoke(nameof(Destroy));
        goContent.transform.SetAsLastSibling();
        transform.SetAsLastSibling();
        BringToFront();
        Invoke(nameof(Hide), mDuration);
    }

    private void OnDestroy()
    {
        DOTween.Kill(rtContent);
    }
}