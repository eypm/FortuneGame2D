using System.Collections.Generic;
using UnityEngine.UI;

public class UpgradePopup : UIPopup
{
    public List<UpgradeItemLayout> upgradeItemLayouts;
    public Button closeButton;
    void Start()
    {
        closeButton.onClick.AddListener(Close);
        Card[] cards = Config.cardDatabase.cards;
        for (int i = 0; i < upgradeItemLayouts.Count; i++)
        {
            upgradeItemLayouts[i].Fill(cards[i]);
        }
    }

    private void Close()
    {
        ClosePopup();
    }
}
