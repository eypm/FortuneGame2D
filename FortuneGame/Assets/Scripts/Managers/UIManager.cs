using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Transform trPopupParent;
    public static List<UIPopup> activePopups = new List<UIPopup>();

    public  void InstantiatePopup(PopupType type, Action onAction = null)
    {
        string prefabName = type.ToString();
        string key = "Prefabs/Popups/" + prefabName + "/" + prefabName;
        GameObject go = (GameObject)Instantiate(Resources.Load(key), trPopupParent);
        UIPopup popup = go.GetComponent<UIPopup>();
        popup.popupType = type;
        popup.onAction = onAction;
        activePopups.Add(popup);
    }

    private void Awake()
    {
        instance = this;
    }
}