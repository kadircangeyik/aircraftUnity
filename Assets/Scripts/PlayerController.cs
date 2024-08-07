using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    [Header("Oyuncu Ayarları")]
    public float hiz = 1f;
    public float yonHizi = 4f;
    private Rigidbody2D rb;
    public GameObject mermiPrefab;
    public Transform atesNoktasi;
    private GameController gameController;
    public float mermiHizi = 10f;
    public float mermiOmru = 2f;
    private bool atesEdebilir = true;
    public float xp;
    public int level;
    private int xpGereken = 100;
    [Range(0, 5)]
    public int saglik = 5;
    private int sayac = 0;
    public int minutes, seconds;
    private float xpArtisZamani = 10f;
    private float xpZamanSayaci = 0f;
    public int yildizSayisi = 0;
    public int envanterdekiYildizSayisi;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameController = FindObjectOfType<GameController>();
        envanterdekiYildizSayisi = PlayerDataManager.Instance.playerStars;

        xpGereken = 100 + (level - 1) * 250;

        InvokeRepeating("UpdateCounter", 1f, 1f);

        if (gameController.xpBar != null)
        {
            gameController.xpBar.maxValue = xpGereken;
            gameController.xpBar.value = xp;
        }

        gameController.atesButonu.onClick.AddListener(OnFireButtonPressed);

        xp = PlayerDataManager.Instance.playerXP;
        level = PlayerDataManager.Instance.playerLevel;
    }

    void Update()
    {
        if (gameController.joystick != null)
        {
            float yatayGiris = gameController.joystick.Horizontal;
            float dikeyGiris = gameController.joystick.Vertical;
            Vector2 hareket = new Vector2(yatayGiris, dikeyGiris).normalized;

            if (hareket != Vector2.zero)
            {
                float aci = Mathf.Atan2(hareket.y, hareket.x) * Mathf.Rad2Deg;
                rb.rotation = Mathf.LerpAngle(rb.rotation, aci, yonHizi * Time.deltaTime);
            }

            rb.velocity = transform.right * hiz;
        }

        if (xp >= xpGereken)
        {
            LevelUp();
        }
    }

    void FixedUpdate()
    {
        rb.position = new Vector2(
            Mathf.Clamp(rb.position.x, -60, 60),
            Mathf.Clamp(rb.position.y, -60, 60)
        );

        if (gameController.xpBar != null)
        {
            gameController.xpBar.value = xp;
        }

        if (gameController.levelText != null)
        {
            gameController.levelText.text = "Lvl: " + level.ToString();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("star") && gameController != null)
        {
            gameController.DecreaseStarCount();
            Destroy(other.gameObject);
            addXp(100);
            yildizSayisi++;
            envanterdekiYildizSayisi++;
            PlayerDataManager.Instance.SavePlayerData(xp, level, envanterdekiYildizSayisi);
        }
    }

    void OnFireButtonPressed()
    {
        if (atesEdebilir)
        {
            Fire();
            atesEdebilir = false;
            Invoke("ResetFire", 0.3f);
        }
    }

    void ResetFire()
    {
        atesEdebilir = true;
    }

    void Fire()
    {
        if (mermiPrefab != null && atesNoktasi != null)
        {
            GameObject mermi = Instantiate(mermiPrefab, atesNoktasi.position, atesNoktasi.rotation);
            Bullet bullet = mermi.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.SetBulletSpeed(mermiHizi);
            }
            Destroy(mermi, mermiOmru);
        }
    }

    void TakeDamage(int damage)
    {
        saglik -= damage;
        if (saglik <= 0)
        {
            Ol();
        }
    }

    void Ol()
    {
        Destroy(gameObject);
        Debug.Log("Öldün!");
    }

    void UpdateCounter()
    {
        sayac++;
        minutes = sayac / 60;
        seconds = sayac % 60;

        xpZamanSayaci += 1f;

        if (xpZamanSayaci >= xpArtisZamani)
        {
            addXp(50);
            xpZamanSayaci = 0f;
        }
    }

    void LevelUp()
    {
        level++;
        xp -= xpGereken;
        xpGereken += level * 250;

        if (gameController.xpBar != null)
        {
            gameController.xpBar.maxValue = xpGereken;
        }

        Debug.Log("Level atladın! Yeni Level: " + level);
        StartCoroutine(LevelUpPanelGosterGizle());
        PlayerDataManager.Instance.SavePlayerData(xp, level, envanterdekiYildizSayisi);
    }

    private IEnumerator LevelUpPanelGosterGizle()
    {
        gameController.levelUpPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        gameController.levelUpPanel.SetActive(false);
    }

    public void addXp(int xpMiktari)
    {
        xp += xpMiktari;
        StartCoroutine(GosterVeKapat(xpMiktari));
        PlayerDataManager.Instance.SavePlayerData(xp, level, envanterdekiYildizSayisi);
    }

    private IEnumerator GosterVeKapat(int xpMiktari)
    {
        gameController.eklenenXpMiktari.text = "+" + xpMiktari;
        gameController.eklenenXpMiktari.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        gameController.eklenenXpMiktari.gameObject.SetActive(false);
    }

    void OnApplicationQuit()
    {
        PlayerDataManager.Instance.SavePlayerData(xp, level, envanterdekiYildizSayisi);
    }
}
