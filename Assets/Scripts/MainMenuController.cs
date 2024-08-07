using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private float xp, level, stars;
    public TMP_Text yildizSayisiText;

    void Start()
    {
        PlayerDataManager.Instance.LoadPlayerData();
        xp = PlayerDataManager.Instance.playerXP;
        level = PlayerDataManager.Instance.playerLevel;
        stars = PlayerDataManager.Instance.playerStars;
        yildizSayisiText.text = stars.ToString();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("game");
    }
}
