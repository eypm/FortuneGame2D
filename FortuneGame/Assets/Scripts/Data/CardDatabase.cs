using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "Game Data/CardDatabase")]
public class CardDatabase : ScriptableObject
{
    public Card[] cards;
}

[Serializable]
public class Card
{
    public CardType cardType;
    public List<CardLevel> levels;


    public int GetUpgradeCost(int level)
    {
        return levels.First(x => x.level == level).upgradeCost;
    }

    public int GetCardPoint(int level)
    {
        return levels.First(x => x.level == level).point;
    }

    public bool IsMaxed(int level)
    {
        return levels.FirstOrDefault(x => x.level == level) == null;
    }
}

[Serializable]
public enum CardType
{
    None = 0,
    Armor = 1,
    Knife = 2,
    Pistol = 3,
    Rifle = 4,
    Shotgun = 5,
    Smg = 6,
    Submachine = 7,
    Vest = 8,
    Sniper = 9,
}

public static class CardTypeExtensions
{
    public static Sprite GetSprite(this CardType cardType)
    {
        string path = cardType switch
        {
            CardType.Armor => "Textures/UpgradePopup/ui_icons_armor_points",
            CardType.Knife => "Textures/UpgradePopup/ui_icons_knife_points",
            CardType.Pistol => "Textures/UpgradePopup/ui_icons_pistol_points",
            CardType.Rifle => "Textures/UpgradePopup/ui_icons_rifle_points",
            CardType.Shotgun => "Textures/UpgradePopup/ui_icons_shotgun_points",
            CardType.Smg => "Textures/UpgradePopup/ui_icons_smg_points",
            CardType.Submachine => "Textures/UpgradePopup/ui_icons_submachine_points",
            CardType.Vest => "Textures/UpgradePopup/ui_icons_vest_points",
            CardType.Sniper => "Textures/UpgradePopup/ui_icons_sniper_points",
            _ => ""
        };
        return Resources.Load<Sprite>(path);
    }
}

[Serializable]
public class CardLevel
{
    public int level;
    public int upgradeCost;
    public int point;
}