using UnityEngine;

public class RandomEnemyController : MonoBehaviour
{
    public float speed = 2f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 5f;
    public float bulletLifetime = 2f;
    public float fireRate = 2f;
    public float attackDistance = 5f;
    public float minDistance = 2f;
    public int patrolPointCount = 4; // Patrol noktası sayısı
    public float patrolRangeX = 120f; // X ekseninde rastgele aralık
    public float patrolRangeY = 120f; // Y ekseninde rastgele aralık
    public float idleTime = 3f;
    public int health = 3;

    private enum State
    {
        MovingTowards,
        MovingAway,
        Attacking,
        Patrolling,
        Hiding
    }

    private State currentState;
    private float nextFireTime;
    private Transform playerTransform;
    private Rigidbody2D rb;
    private Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    private float idleStartTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Oyuncu bulunamadı.");
        }

        // Randomize behavior
        currentState = (State)Random.Range(0, System.Enum.GetValues(typeof(State)).Length);

        // Generate random patrol points
        patrolPoints = new Transform[patrolPointCount];
        for (int i = 0; i < patrolPointCount; i++)
        {
            GameObject patrolPointObj = new GameObject("PatrolPoint" + i);
            patrolPointObj.transform.position = new Vector3(
                Random.Range(-patrolRangeX, patrolRangeX),
                Random.Range(-patrolRangeY, patrolRangeY),
                0
            );
            patrolPoints[i] = patrolPointObj.transform;
        }

        // Set idle time if hiding
        if (currentState == State.Hiding)
        {
            idleStartTime = Time.time + idleTime;
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            switch (currentState)
            {
                case State.MovingTowards:
                    if (distanceToPlayer > minDistance)
                    {
                        MoveTowardsPlayer();
                    }
                    else
                    {
                        currentState = State.MovingAway;
                    }
                    break;

                case State.MovingAway:
                    if (distanceToPlayer <= minDistance)
                    {
                        MoveAwayFromPlayer();
                    }
                    else
                    {
                        currentState = State.Attacking;
                    }
                    break;

                case State.Attacking:
                    if (distanceToPlayer <= attackDistance)
                    {
                        Fire();
                        currentState = State.MovingAway;
                    }
                    else
                    {
                        currentState = State.MovingTowards;
                    }
                    break;

                case State.Patrolling:
                    Patrol();
                    break;

                case State.Hiding:
                    if (Time.time > idleStartTime)
                    {
                        currentState = Random.Range(0, 2) == 0 ? State.MovingTowards : State.MovingAway;
                    }
                    break;
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.velocity = direction * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = Mathf.LerpAngle(rb.rotation, angle, speed * Time.deltaTime);
    }

    void MoveAwayFromPlayer()
    {
        Vector2 direction = (transform.position - playerTransform.position).normalized;
        rb.velocity = direction * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = Mathf.LerpAngle(rb.rotation, angle, speed * Time.deltaTime);
    }

    void Fire()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            if (Time.time > nextFireTime)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                if (bulletRb != null)
                {
                    bulletRb.velocity = firePoint.up * bulletSpeed;
                }
                Destroy(bullet, bulletLifetime);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length > 0)
        {
            Transform target = patrolPoints[currentPatrolIndex];
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * speed;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = Mathf.LerpAngle(rb.rotation, angle, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, target.position) < 0.1f)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Düşman öldü!");
        Destroy(gameObject);
    }
}
