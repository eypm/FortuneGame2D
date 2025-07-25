using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpinDatabase", menuName = "Game Data/SpinDatabase")]
public class SpinDatabase : ScriptableObject
{
    [Header("Zone Datas")] public ZoneData[] zoneDatas;
}

[Serializable]
public class ZoneData
{
    public int zoneLevel;
    public bool isSafeZone;
    public Reward[] rewards;
    public float bombChance;
    public ZoneType zoneType;
}

public enum ZoneType
{
    None = 0,
    Normal = 1,
    Silver = 2,
    Golden = 3,
}

public static class ZoneTypeExtensions
{
    public static Sprite GetZoneBgSprite(this ZoneType zoneType)
    {
        string path = zoneType switch
        {
            ZoneType.Normal => "Textures/SpinPopup/ui_spin_bronze_base",
            ZoneType.Silver => "Textures/SpinPopup/ui_spin_silver_base",
            ZoneType.Golden => "Textures/SpinPopup/ui_spin_golden_base",
            _ => ""
        };
        return Resources.Load<Sprite>(path);
    }

    public static Sprite GetZoneIndicatorSprite(this ZoneType zoneType)
    {
        string path = zoneType switch
        {
            ZoneType.Normal => "Textures/SpinPopup/ui_spin_bronze_indicator",
            ZoneType.Silver => "Textures/SpinPopup/ui_spin_silver_indicator",
            ZoneType.Golden => "Textures/SpinPopup/ui_spin_golden_indicator",
            _ => ""
        };
        return Resources.Load<Sprite>(path);
    }

    public static string GetZoneTitle(this ZoneType zoneType)
    {
        string title = zoneType switch
        {
            ZoneType.Normal => "Bronze Spin",
            ZoneType.Silver => "Silver Spin",
            ZoneType.Golden => "Golden Spin",
            _ => ""
        };
        return title;
    }
}

[Serializable]
public class Reward
{
    public int rewardAmount;
    public RewardType rewardType;
}

public enum RewardType
{
    None = 0,
    Coin = 1,
    Money = 2,
    Bomb = 3,
    BigChest = 4,
    BronzeChest = 5,
    GoldChest = 6,
    SilverChest = 7,
    SmallChest = 8,
    StandardChest = 9,
    SuperChest = 10,
}

public static class RewardTypeExtensions
{
    public static Sprite GetSprite(this RewardType rewardType)
    {
        string path = rewardType switch
        {
            RewardType.Coin => "Textures/General/ui_icon_gold",
            RewardType.Money => "Textures/General/ui_icon_cash",
            RewardType.Bomb => "Textures/SpinPopup/ui_icon_bomb",
            RewardType.BigChest => "Textures/Chests/ui_icon_chest_big_nolight",
            RewardType.BronzeChest => "Textures/Chests/ui_icon_chest_bronze_nolight",
            RewardType.GoldChest => "Textures/Chests/ui_icon_chest_gold_nolight",
            RewardType.SilverChest => "Textures/Chests/ui_icon_chest_silver_nolight",
            RewardType.SmallChest => "Textures/Chests/ui_icon_chest_small_nolight",
            RewardType.StandardChest => "Textures/Chests/ui_icon_chest_standard_nolight",
            RewardType.SuperChest => "Textures/Chests/ui_icon_chest_super_nolight",
            _ => ""
        };
        return Resources.Load<Sprite>(path);
    }

    public static ChestType GetChestType(this RewardType rewardType)
    {
        ChestType type = rewardType switch
        {
            RewardType.SmallChest => ChestType.SmallChest,
            RewardType.StandardChest => ChestType.StandardChest,
            RewardType.SuperChest => ChestType.SuperChest,
            RewardType.BigChest => ChestType.BigChest,
            RewardType.BronzeChest => ChestType.BronzeChest,
            RewardType.GoldChest => ChestType.GoldChest,
            RewardType.SilverChest => ChestType.SilverChest,
            _ => ChestType.None
        };
        return type;
    }

    public static bool IsChestReward(this RewardType rewardType)
    {
        bool isChestReward = rewardType switch
        {
            RewardType.Coin => false,
            RewardType.Money => false,
            RewardType.Bomb => false,
            _ => true
        };
        return isChestReward;
    }

    public static Vector2 GetSpinItemSpriteSize(this RewardType rewardType)
    {
        Vector2 size = rewardType switch
        {
            RewardType.Coin => new Vector2(130, 77),
            RewardType.Money => new Vector2(120, 65),
            RewardType.Bomb => new Vector2(75, 100),
            RewardType.BigChest => new Vector2(100, 50),
            RewardType.BronzeChest => new Vector2(100, 50),
            RewardType.GoldChest => new Vector2(100, 50),
            RewardType.SilverChest => new Vector2(100, 50),
            RewardType.SmallChest => new Vector2(100, 50),
            RewardType.StandardChest => new Vector2(100, 50),
            RewardType.SuperChest => new Vector2(100, 50),
            _ => new Vector2(100, 100),
        };
        return size;
    }

    public static Vector2 GetSpinRewardPanelSpriteSize(this RewardType rewardType)
    {
        Vector2 size = rewardType switch
        {
            RewardType.Coin => new Vector2(512,270),
            RewardType.Money => new Vector2(512,270),
           _ => new Vector2(512, 228),
        };
        return size;
    }
}