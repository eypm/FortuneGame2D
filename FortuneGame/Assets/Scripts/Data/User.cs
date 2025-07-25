using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class User
{
    public List<CardItem> cardItems = new List<CardItem>();
    public List<ChestItem> chestItems = new List<ChestItem>();
    public int zoneLevel;
    public int money;
    public int gold;
    public int loseAllCount;
    public UnityEvent OnCurrencyAmountChanged = new UnityEvent();

    public static User CreateNew()
    {
        List<CardItem> cardItems = new List<CardItem>();
        cardItems.Add(new CardItem { type = CardType.Armor, level = 1, point = 100 });
        cardItems.Add(new CardItem { type = CardType.Knife, level = 1, point = 0 });
        cardItems.Add(new CardItem { type = CardType.Pistol, level = 1, point = 0 });
        cardItems.Add(new CardItem { type = CardType.Rifle, level = 1, point = 0 });
        cardItems.Add(new CardItem { type = CardType.Shotgun, level = 1, point = 0 });
        cardItems.Add(new CardItem { type = CardType.Smg, level = 1, point = 0 });
        cardItems.Add(new CardItem { type = CardType.Submachine, level = 1, point = 0 });
        cardItems.Add(new CardItem { type = CardType.Vest, level = 1, point = 100 });
        cardItems.Add(new CardItem { type = CardType.Sniper, level = 1, point = 0 });

        List<ChestItem> chestItems = new List<ChestItem>();
        chestItems.Add(new ChestItem { type = ChestType.SmallChest, level = 1 });
        chestItems.Add(new ChestItem { type = ChestType.StandardChest, level = 1 });
        chestItems.Add(new ChestItem { type = ChestType.BronzeChest, level = 1 });
        chestItems.Add(new ChestItem { type = ChestType.SilverChest, level = 1 });
        chestItems.Add(new ChestItem { type = ChestType.BigChest, level = 1 });
        chestItems.Add(new ChestItem { type = ChestType.GoldChest, level = 1 });
        chestItems.Add(new ChestItem { type = ChestType.SuperChest, level = 1 });
        User user = new User
        {
            zoneLevel = 1,
            cardItems = cardItems,
            chestItems = chestItems,
            money = 5000000,
            gold = 10000,
            loseAllCount = 0,
        };
        return user;
    }

    public void UpdateLoseAllCount(int value)
    {
        loseAllCount = value;
        Save();
    }

    public void UpdateMoney(int amount)
    {
        money += amount;
        OnCurrencyAmountChanged?.Invoke();
        Save();
    }

    public void UpdateGold(int amount)
    {
        money += amount;
        OnCurrencyAmountChanged?.Invoke();
        Save();
    }

    public void IncrementZoneLevel(int amount = 1)
    {
        zoneLevel += amount;
        Save();
    }


    #region Card

    public void IncrementCardPoint(CardType cardType, int point)
    {
        cardItems.First(x => x.type == cardType).point += point;
        Save();
    }

    public int GetCardLevel(CardType cardType)
    {
        return cardItems.First(x => x.type == cardType).level;
    }

    public int GetCardPoint(CardType cardType)
    {
        return cardItems.First(x => x.type == cardType).point;
    }


    public void IncrementCardLevel(CardType cardType)
    {
        cardItems.First(x => x.type == cardType).level++;
        Save();
    }

    [Serializable]
    public class CardItem
    {
        public CardType type;

        public int level;

        public int point;
    }

    #endregion

    #region Chest

    public int GetChestLevel(ChestType chestType)
    {
        return chestItems.First(x => x.type == chestType).level;
    }

    public void IncrementChestLevel(ChestType chestType)
    {
        chestItems.First(x => x.type == chestType).level++;
        Save();
    }

    [Serializable]
    public class ChestItem
    {
        public ChestType type;
        public int level;
    }

    #endregion


    private void Save()
    {
        PreferenceHelper.SaveUserToPrefs(this);
    }
}