using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class SelectableBehaviour : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler,
    IPointerClickHandler
{
    public Selectable selectable;
    public RectTransform rtAnimation;
    public float pressedSize = 0.95f;
    public float normalSize = 1;

    private void OnDestroy()
    {
        DOTween.Kill(rtAnimation);
    }


    private void Animate(float size)
    {
        if (selectable == null) return;
        if (!selectable.interactable) return;
        if (rtAnimation != null)
            rtAnimation.DOScale(size, 0.05f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Animate(pressedSize);
        AudioManager.GetInstance().PlaySound(AudioManager.Sounds.buttonClick);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Animate(normalSize);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Animate(normalSize);
    }
}