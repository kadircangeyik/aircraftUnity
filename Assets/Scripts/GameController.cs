using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject starPrefab;  // Yıldız prefabı
    public GameObject playerPrefab; // Player prefabı
    public int numberOfStars = 10; // Başlangıçta spawn edilecek yıldız sayısı
    public Vector2 spawnAreaSize = new Vector2(10, 10); // Yıldızların spawn olacağı bölgenin boyutu
    public Vector2 playerSpawnPosition = new Vector2(0, 0); // Oyuncunun spawn olacağı pozisyon

    public TMP_Text counterText; 
    // UI
    public GameObject yildizUI;
    public TMP_Text yildizSayisiTxt;
    private int yildizSayisi = 0;
    public PlayerController playerController;
    public TMP_Text levelText;
    public TMP_Text levelUpText;
    
    public GameObject levelUpPanel;
    void Start()
    {
        SpawnPlayer(); // Oyuncuyu spawn et
        SpawnStars(numberOfStars); // Yıldızları spawn et
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
        YildizSayisiArtti();
    }

    void YildizSayisiArtti()
    {
        yildizSayisi++;
        yildizSayisiTxt.text = yildizSayisi.ToString();
        StartCoroutine(YildizGosterGizle());
    }

    private IEnumerator YildizGosterGizle()
    {
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
        if (counterText != null){
         counterText.text = zaman;
        }
        else{
            Debug.Log("Hata, counterText null görünüyor!");
        }
        levelText.text = "Lvl " + playerController.level.ToString();
        levelUpText.text = "New Level:  " + playerController.level.ToString();
    }
}
