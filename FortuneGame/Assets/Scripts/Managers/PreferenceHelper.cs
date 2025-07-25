using UnityEngine;

public class PreferenceHelper
{
    private enum Key
    {
        SOUND_ON,
        USER,
    }
   

    private static void SetString(Key key, string value)
    {
        if (value == null)
        {
            DeleteKey(key);
        }
        else
        {
            PlayerPrefs.SetString(key.ToString(), value);
        }
    }

    private static void DeleteKey(Key key)
    {
        PlayerPrefs.DeleteKey(key.ToString());
    }

    private static int GetInt(Key key, int defaultValue = -1)
    {
        return  PlayerPrefs.GetInt(key.ToString(),defaultValue);

        
    }

    private static void SetInt(Key key, int value)
    {
        PlayerPrefs.SetInt(key.ToString(),value);

    }

    public static void SetSoundEnabled(bool enabled)
    {
        SetInt(Key.SOUND_ON, enabled ? 1 : 0);
    }

    public static bool IsSoundEnabled()
    {
        return GetInt(Key.SOUND_ON, 1) == 1;
    }


    public static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void SaveUserToPrefs(User user)
    {
        string json = JsonUtility.ToJson(user);
        PlayerPrefs.SetString(nameof(Key.USER), json);
        PlayerPrefs.Save();
        Debug.Log("Kullanıcı verisi kaydedildi: " + json);
    }

    public static User GetUserFromPrefs()
    {
        if (!PlayerPrefs.HasKey(nameof(Key.USER)))
        {
            Debug.LogWarning("Kullanıcı verisi bulunamadı, yeni kullanıcı oluşturuluyor.");
            return User.CreateNew();
        }

        string json = PlayerPrefs.GetString(nameof(Key.USER));
        User user = JsonUtility.FromJson<User>(json);
        Debug.Log("Kullanıcı verisi yüklendi: " + json);
        return user;
    }
    
}