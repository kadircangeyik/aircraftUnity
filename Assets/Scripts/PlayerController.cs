using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    [Header("Oyuncu Ayarları")]
    public float hiz = 1f;
    public float yonHizi = 4f; // Oyuncu hızları
    private Rigidbody2D rb; // Oyuncunun Rigidbody'si
    private Joystick joystick; // Joystick
    public Button atesButonu; // Ateş etme butonu
    public GameObject mermiPrefab; // Mermi nesnesi
    public Transform atesNoktasi; // Merminin çıkış noktası
    private GameController gameController; // Oyun kontrol scripti
    public float mermiHizi = 10f; // Mermilerin hızı
    public float mermiOmru = 2f; // Mermilerin yaşam süresi
    private bool atesEdebilir = true; // Butona basıldığında bir kez ateş etmeyi sağlamak için
    public float xp;
    public int level;
    public Slider xpBar; // XP barı
    private int xpGereken; // Level atlamak için gereken XP miktarı
    [Range(0, 5)]
    public int saglik = 5; // Oyuncunun sağlığı
    private int sayac = 0;
    public int minutes,seconds;

    void Start()
    {
        xp = 0;
        level = 1;
        xpGereken = 100; // İlk level için gereken XP
        rb = GetComponent<Rigidbody2D>();
        joystick = FindObjectOfType<Joystick>();
        gameController = FindObjectOfType<GameController>();
        xpBar = GameObject.Find("xpBar")?.GetComponent<Slider>();
        // Her saniyede bir sayaç güncelle
        InvokeRepeating("UpdateCounter", 1f, 1f);

        atesButonu = GameObject.Find("Split")?.GetComponent<Button>();

        if (atesButonu != null)
        {
            atesButonu.onClick.AddListener(OnFireButtonPressed);
        }
        else
        {
            Debug.LogError("Ateş Butonu bulunamadı");
        }

        // Kontrol et ve hata mesajı yazdır
        if (joystick == null)
        {
            Debug.LogError("Joystick bulunamadı");
        }

        if (gameController == null)
        {
            Debug.LogError("OyunKontrol bulunamadı");
        }

        if (xpBar != null)
        {
            xpBar.maxValue = xpGereken;
            xpBar.value = xp;
        }
    }

    void Update()
    {
        if (joystick != null)
        {
            // Joystick girişlerini oku
            float yatayGiris = joystick.Horizontal;
            float dikeyGiris = joystick.Vertical;

            // Hareket vektörünü hesapla
            Vector2 hareket = new Vector2(yatayGiris, dikeyGiris).normalized;

            // Uçaksavarın döneceği yönü hesapla
            if (hareket != Vector2.zero)
            {
                float aci = Mathf.Atan2(hareket.y, hareket.x) * Mathf.Rad2Deg;
                rb.rotation = Mathf.LerpAngle(rb.rotation, aci, yonHizi * Time.deltaTime);
            }

            // Sürekli ileri hareket
            rb.velocity = transform.right * hiz;
        }

        // XP kontrolü ve level atlama
        if (xp >= xpGereken)
        {
            LevelUp();
        }

        if (xpBar != null)
        {
            xpBar.value = xp;
        }
    }

    void FixedUpdate()
    {
        // Sınırlar içinde kalmayı sağlama
        rb.position = new Vector2(
            Mathf.Clamp(rb.position.x, -60, 60),
            Mathf.Clamp(rb.position.y, -60, 60)
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("star") && gameController != null)
        {
            gameController.DecreaseStarCount();
            Destroy(other.gameObject);
            xp += 100;
        }
    }

    void OnFireButtonPressed()
    {
        if (atesEdebilir)
        {
            Fire();
            atesEdebilir = false;
            Invoke("ResetFire", 0.3f); // Butona tekrar basılmasını engellemek için kısa bir süre bekle
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
                bullet.SetBulletSpeed(mermiHizi); // Mermiye hız atama
            }
            Destroy(mermi, mermiOmru); // Belirli bir süre sonra mermiyi yok et
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
        sayac++;  // Sayacı arttır
          // Dakika ve saniye hesaplama
        minutes = sayac / 60;
        seconds = sayac % 60;
        xp += 10;
    }

    void LevelUp()
    {
        level++;
        xp -= xpGereken; // XP sıfırlanır ve kalan XP yeni levele eklenir
        xpGereken += level * 250; // Her level için gereken XP miktarı arttırılır

        if (xpBar != null)
        {
            xpBar.maxValue = xpGereken;
        }

        Debug.Log("Level atladın! Yeni Level: " + level);
         StartCoroutine(levelUpPanelGosterGizle());
    }
     private IEnumerator levelUpPanelGosterGizle()
    {
        gameController.levelUpPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        gameController.levelUpPanel.SetActive(false);
    }
}
