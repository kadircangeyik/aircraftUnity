using UnityEngine;

public class playerFollow : MonoBehaviour
{
    public Transform hedefOyuncu;  // Takip edilecek hedef (oyuncu)
    public Vector3 offset;    // Kameranın hedefe göre ofseti
    public float kameraYumusakligi = 0.125f;  // Kamera hareketinin yumuşaklığı

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            hedefOyuncu = player.transform;
        }
        else
        {
            Debug.LogError("Oyuncu bulunamadı.");
        }
    }

    void LateUpdate()
    {
        if (hedefOyuncu != null)
        {
            Vector3 istenenPozisyon = hedefOyuncu.position + offset;

            Vector3 yumusatilmisKonum = Vector3.Lerp(transform.position, istenenPozisyon, kameraYumusakligi);

            // Kameranın pozisyonunu güncelle
            transform.position = new Vector3(yumusatilmisKonum.x, yumusatilmisKonum.y, transform.position.z);
        }
    }
}
