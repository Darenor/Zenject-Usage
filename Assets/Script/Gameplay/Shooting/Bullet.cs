
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour, IPoolable<BulletSpawnData, IMemoryPool>
{
    [Header("Bullet Settings")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float lifetime = 3f;

    private Rigidbody2D rb;

    private TargetData targetData;
    private Transform targetTransform;

    private IMemoryPool pool;

    private AudioSource audioSource;
    private AudioClip hitClip;

    private float remainingLifetime;
    private bool isActive;

    private Vector2 direction;

    [Inject]
    private void Construct(
        AudioSource audioSource,
        [Inject(Id = "HitClip")] AudioClip hitClip)
    {
        this.audioSource = audioSource;
        this.hitClip = hitClip;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnSpawned(
        BulletSpawnData spawnData,
        IMemoryPool pool)
    {
        this.pool = pool;

        targetData = spawnData.TargetData;
        targetTransform = spawnData.TargetTransform;

        remainingLifetime = lifetime;
        isActive = true;

        gameObject.SetActive(true);

        UpdateDirection();
    }

    public void OnDespawned()
    {
        isActive = false;

        targetData = null;
        targetTransform = null;
        pool = null;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    private void Update()
    {
        if (!isActive)
            return;

        remainingLifetime -= Time.deltaTime;

        if (remainingLifetime <= 0f)
        {
            ReturnToPool();
        }
    }

    private void FixedUpdate()
    {
        if (!isActive)
            return;

        UpdateDirection();

        if (direction.sqrMagnitude > 0.001f)
        {
            rb.MovePosition(
                rb.position +
                direction * speed * Time.fixedDeltaTime
            );
        }
    }

    private void UpdateDirection()
    {
        Transform currentTarget = targetTransform;

        if (currentTarget == null && targetData != null)
        {
            currentTarget = targetData.TargetTransform;
        }

        if (currentTarget == null)
            return;

        Vector2 difference =
            (Vector2)currentTarget.position - rb.position;

        if (difference.sqrMagnitude > 0.001f)
        {
            direction = difference.normalized;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive)
            return;

        DestructibleObstacle obstacle =
            other.GetComponentInParent<DestructibleObstacle>();

        if (obstacle != null)
        {
            PlayHitSound();
            obstacle.DestroyObstacle();

            ReturnToPool();
            return;
        }

        ReturnToPool();
    }

    private void PlayHitSound()
    {
        if (audioSource != null && hitClip != null)
        {
            audioSource.PlayOneShot(hitClip);
        }
    }

    private void ReturnToPool()
    {
        if (!isActive)
            return;

        pool.Despawn(this);
    }

    public class Factory : PlaceholderFactory<BulletSpawnData, Bullet>
    {
    }
    
    public class Pool : MonoPoolableMemoryPool<BulletSpawnData, IMemoryPool, Bullet>
    {
    }
}
