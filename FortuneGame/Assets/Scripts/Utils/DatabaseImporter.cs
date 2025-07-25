using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;


public class DatabaseImporter : MonoBehaviour
{
    #region SpinDatabaseImporter

    [MenuItem("Tools/Update SpinDatabase (Simplified)")]
    public static void UpdateSpinDatabase()
    {
        string jsonPath = EditorUtility.OpenFilePanel("Select spin_database.json", "", "json");
        if (string.IsNullOrEmpty(jsonPath)) return;

        string jsonContent = File.ReadAllText(jsonPath);

        SpinDatabase spinDatabase =
            AssetDatabase.LoadAssetAtPath<SpinDatabase>("Assets/Resources/Data/SpinDatabase.asset");
        if (spinDatabase == null)
        {
            Debug.LogError("SpinDatabase.asset not found at path.");
            return;
        }

        ZoneRoot zoneRoot = JsonUtility.FromJson<ZoneRoot>(jsonContent);
        spinDatabase.zoneDatas = zoneRoot.zoneDatas.ToArray();
        EditorUtility.SetDirty(spinDatabase);
        AssetDatabase.SaveAssets();
        Debug.Log("SpinDatabase updated.");
    }

    [System.Serializable]
    private class ZoneRoot
    {
        public List<ZoneData> zoneDatas;
    }

    #endregion

    #region CardDatabaseImporter

    [MenuItem("Tools/Update CardDataBase (Simplified)")]
    public static void UpdateCardDatabase()
    {
        string jsonPath = EditorUtility.OpenFilePanel("Select card_database.json", "", "json");
        if (string.IsNullOrEmpty(jsonPath)) return;

        string jsonContent = File.ReadAllText(jsonPath);

        CardDatabase cardDatabase =
            AssetDatabase.LoadAssetAtPath<CardDatabase>("Assets/Resources/Data/CardDatabase.asset");
        if (cardDatabase == null)
        {
            Debug.LogError("CardDatabase.asset not found at path.");
            return;
        }

        cardDatabase.cards = JsonUtility.FromJson<Wrapper<Card>>(WrapJson("cards", jsonContent)).items.ToArray();

        EditorUtility.SetDirty(cardDatabase);
        AssetDatabase.SaveAssets();
        Debug.Log("CardDatabase updated.");
    }

    private static string WrapJson(string key, string json)
    {
        return $"{{\"items\": {json}}}";
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public List<T> items;
    }

    #endregion

    #region ChestDatabaseImporter

    [MenuItem("Tools/Update ChestDatabase (Simplified)")]
    public static void UpdateChestDatabase()
    {
        string jsonPath = EditorUtility.OpenFilePanel("Select chest_database.json", "", "json");
        if (string.IsNullOrEmpty(jsonPath)) return;

        string jsonContent = File.ReadAllText(jsonPath);

        ChestDatabase chestDatabase =
            AssetDatabase.LoadAssetAtPath<ChestDatabase>("Assets/Resources/Data/ChestDatabase.asset");
        if (chestDatabase == null)
        {
            Debug.LogError("ChestDatabase.asset not found at path.");
            return;
        }

        // Custom wrapper class that matches actual JSON structure
        ChestRoot chestRoot = JsonUtility.FromJson<ChestRoot>(jsonContent);
        chestDatabase.chests = chestRoot.chests.ToArray();

        EditorUtility.SetDirty(chestDatabase);
        AssetDatabase.SaveAssets();
        Debug.Log("ChestDatabase updated.");
    }

    [System.Serializable]
    private class ChestRoot
    {
        public List<Chest> chests;
    }

    #endregion
}