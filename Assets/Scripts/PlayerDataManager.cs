using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }
    public float playerXP { get; private set; }
    public int playerLevel { get; private set; }
    public int playerStars { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadPlayerData();
    }

    public void SavePlayerData(float xp, int level, int stars)
    {
        Debug.Log($"Saving data: XP = {xp}, Level = {level}, Stars = {stars}");
        PlayerPrefs.SetFloat("PlayerXP", xp);
        PlayerPrefs.SetInt("PlayerLevel", level);
        PlayerPrefs.SetInt("PlayerStars", stars);
        PlayerPrefs.Save();
    }

    public void LoadPlayerData()
    {
        playerXP = PlayerPrefs.GetFloat("PlayerXP", 0);
        playerLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        playerStars = PlayerPrefs.GetInt("PlayerStars", 0); // Varsayılan değer olarak 0 kullanılmış
        Debug.Log($"Loaded data: XP = {playerXP}, Level = {playerLevel}, Stars = {playerStars}");
    }
}
