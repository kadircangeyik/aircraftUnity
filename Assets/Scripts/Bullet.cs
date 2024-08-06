using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Merminin Hızı
    private float mermiHizi = 2f;

    //Merminin Hızını diğer scriptler üzerinden kontrol etmek için
    public void SetBulletSpeed(float hiz)
    {
        mermiHizi = hiz;
    }
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Rigibody hizi sağ transform * mermi hizi
            rb.velocity = transform.right * mermiHizi;
        }
    }
}
