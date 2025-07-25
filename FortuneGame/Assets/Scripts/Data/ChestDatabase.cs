using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ChestDatabase", menuName = "Game Data/ChestDatabase")]
public class ChestDatabase : ScriptableObject
{
    public Chest[] chests;


    private ChestLevel[] GetChestLevels(ChestType chestType)
    {
        return chests.FirstOrDefault(x => x.chestType == chestType).levels;
    }

    public ChestLevel GetChestLevel(ChestType chestType)
    {
        ChestLevel[] chestLevels = GetChestLevels(chestType);
        return chestLevels.FirstOrDefault(x => x.level == App.user.GetChestLevel(chestType));
    }
}

[Serializable]
public class Chest
{
    public ChestType chestType;
    public ChestLevel[] levels;
}
[Serializable]
public enum ChestType
{
    None = 0,
    SmallChest = 1,
    StandardChest = 2,
    BronzeChest = 3,
    BigChest = 4,
    SilverChest = 5,
    GoldChest = 6,
    SuperChest = 7,
}

public static class ChestTypeExtensions
{
    public static Sprite GetSprite(this ChestType chestType)
    {
        string path = chestType switch
        {
            ChestType.SmallChest => "Textures/Chests/ui_icon_chest_small_nolight",
            ChestType.StandardChest => "Textures/Chests/ui_icon_chest_standard_nolight",
            ChestType.BronzeChest => "Textures/Chests/ui_icon_chest_bronze_nolight",
            ChestType.BigChest => "Textures/Chests/ui_icon_chest_big_nolight",
            ChestType.SilverChest => "Textures/Chests/ui_icon_chest_silver_nolight",
            ChestType.GoldChest => "Textures/Chests/ui_icon_chest_gold_nolight",
            ChestType.SuperChest => "Textures/Chests/ui_icon_chest_super_nolight",
            _ => ""
        };
        return Resources.Load<Sprite>(path);
    }
}

[Serializable]
public class ChestLevel
{
    public int level;
    public ChestReward[] rewards;
}

[Serializable]
public class ChestReward
{
    public CardType cardType;
    public int point;
}