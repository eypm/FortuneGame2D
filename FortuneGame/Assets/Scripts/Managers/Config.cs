using System;
using UnityEngine;

public class Config : MonoBehaviour
{
    public static Config instance;
    [NonSerialized] public static SpinDatabase spinDatabase;
    [NonSerialized] public static CardDatabase cardDatabase;
    [NonSerialized] public static ChestDatabase chestDatabase;
    [NonSerialized] public static ReviveAmountData reviveAmounData;

    void Awake()
    {
        instance = this;
        spinDatabase = Resources.Load<SpinDatabase>("Data/SpinDatabase");
        cardDatabase = Resources.Load<CardDatabase>("Data/CardDatabase");
        chestDatabase = Resources.Load<ChestDatabase>("Data/ChestDatabase");
        reviveAmounData = Resources.Load<ReviveAmountData>("Data/ReviveData");
    }
}