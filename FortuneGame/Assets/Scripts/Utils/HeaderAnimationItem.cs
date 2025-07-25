using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HeaderAnimationItem : MonoBehaviour
{
    public GameObject thisObject;
    public Image image;
    public RectTransform rectTransform;
    public GameObject destroyParticle;
    public Canvas canvas;

    private void Start()
    {
        destroyParticle.SetActive(false);
    }

    public void ShowParticle()
    {
        if (!thisObject.activeInHierarchy)
            Destroy(gameObject);
        else
            DestroyDelay();
    }

    void DestroyDelay()
    {
        image.enabled = false;
        destroyParticle.SetActive(true);
        App.instance.WaitForSeconds(0.3f, () => Destroy(thisObject));
    }
}