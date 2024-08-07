using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject starPrefab;
    public GameObject playerPrefab;
    public int numberOfStars = 10;
    public Vector2 spawnAreaSize = new Vector2(10, 10);
    public Vector2 playerSpawnPosition = new Vector2(0, 0);

    public TMP_Text counterText;
    public GameObject yildizUI;
    public TMP_Text yildizSayisiTxt;

    public PlayerController playerController;
    public TMP_Text levelText;
    public TMP_Text levelUpText;
    public Joystick joystick;
    public Slider xpBar;
    public GameObject levelUpPanel;
    public TMP_Text eklenenXpMiktari;
    public Button atesButonu;

    public void Kaydet()
    {
        PlayerDataManager.Instance.SavePlayerData(playerController.xp, playerController.level, playerController.envanterdekiYildizSayisi);
    }

    void Start()
    {
        SpawnPlayer();
        SpawnStars(numberOfStars);
        InvokeRepeating("UpdateSayacText", 1f, 1f);
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void SpawnStars(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnStar();
        }
    }

    void SpawnStar()
    {
        Vector2 spawnPosition = new Vector2(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2)
        );

        Instantiate(starPrefab, spawnPosition, Quaternion.identity);
    }

    public void DecreaseStarCount()
    {
        numberOfStars = Mathf.Max(0, numberOfStars - 1);
        SpawnStar();
        StartCoroutine(YildizGosterGizle());
    }

    private IEnumerator YildizGosterGizle()
    {
        yildizSayisiTxt.text = playerController.yildizSayisi.ToString();
        yildizUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        yildizUI.SetActive(false);
    }

    void SpawnPlayer()
    {
        Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
    }

    void UpdateSayacText()
    {
        string zaman = string.Format("{0:0}:{1:00}", playerController.minutes, playerController.seconds);
        if (counterText != null)
        {
            counterText.text = zaman;
        }
        else
        {
            Debug.Log("Hata, counterText null görünüyor!");
        }
        levelUpText.text = "New Level:  " + playerController.level.ToString();
    }
}
