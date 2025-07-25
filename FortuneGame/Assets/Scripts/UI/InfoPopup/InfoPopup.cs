using UnityEngine.UI;

public class InfoPopup : UIPopup
{
    public Button btnClose;
    public Button btnYes;
    public Button btnNo;

    private void Start()
    {
        btnClose.onClick.AddListener(ClosePopup);
        btnYes.onClick.AddListener(() =>
        {
            onAction?.Invoke();
            CloseAllPopup();
        });
        btnNo.onClick.AddListener(ClosePopup);
    }
}