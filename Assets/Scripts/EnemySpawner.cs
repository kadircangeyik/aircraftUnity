using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Düşman prefabı
    public int numberOfEnemies = 5; // Spawn edilecek düşman sayısı
    public Vector2 spawnAreaSize = new Vector2(10, 10); // Düşmanların spawn olacağı bölgenin boyutu

    void Start()
    {
        SpawnEnemies(numberOfEnemies);
    }

    void SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 spawnPosition = new Vector2(
                Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2)
            );

            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
