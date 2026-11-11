
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerShooter : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Input")]
    [SerializeField] private InputActionAsset inputActions;

    private InputAction shootAction;

    private Bullet.Factory bulletFactory;
    private TargetData targetData;

    private AudioSource audioSource;
    private AudioClip shootClip;

    [Inject]
    private void Construct(
        Bullet.Factory bulletFactory,
        TargetData targetData,
        AudioSource audioSource,
        [Inject(Id = "ShootClip")] AudioClip shootClip)
    {
        this.bulletFactory = bulletFactory;
        this.targetData = targetData;
        this.audioSource = audioSource;
        this.shootClip = shootClip;
    }

    private void Awake()
    {
        if (inputActions == null)
        {
            Debug.LogError("Input Actions не назначен!", this);
            return;
        }

        shootAction = inputActions
            .FindActionMap("Player")
            .FindAction("Attack");
    }

    private void OnEnable()
    {
        if (shootAction == null)
            return;

        shootAction.Enable();
        shootAction.performed += OnShoot;
    }

    private void OnDisable()
    {
        if (shootAction == null)
            return;

        shootAction.performed -= OnShoot;
        shootAction.Disable();
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        Shoot();
    }

    private void Shoot()
    {
        if (firePoint == null || targetData == null)
            return;

        Transform selectedTarget = FindNearestObstacle();

        if (selectedTarget == null)
            selectedTarget = targetData.TargetTransform;

        BulletSpawnData spawnData = new BulletSpawnData(
            targetData,
            selectedTarget
        );

        Bullet bullet = bulletFactory.Create(spawnData);

        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.identity;

        if (audioSource != null && shootClip != null)
            audioSource.PlayOneShot(shootClip);
    }

    private Transform FindNearestObstacle()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            firePoint.position,
            detectionRadius,
            obstacleLayer
        );

        Transform nearest = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider2D col in colliders)
        {
            DestructibleObstacle obstacle =
                col.GetComponentInParent<DestructibleObstacle>();

            if (obstacle == null)
                continue;

            float distance = Vector2.Distance(
                firePoint.position,
                obstacle.transform.position
            );

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = obstacle.transform;
            }
        }

        return nearest;
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            firePoint.position,
            detectionRadius
        );
    }
}
