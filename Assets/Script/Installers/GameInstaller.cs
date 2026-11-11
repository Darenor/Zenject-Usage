
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Header("Player")]
    [SerializeField] private PlayerShooter playerShooter;

    [Header("Target")]
    [SerializeField] private TargetData targetData;

    [Header("Bullet Pool")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform bulletPoolRoot;
    [SerializeField] private int initialBulletCount = 10;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip hitClip;

    [Header("Obstacle Spawner")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private Transform obstaclesRoot;

    public override void InstallBindings()
    {
        Container.Bind<TargetData>()
            .FromInstance(targetData)
            .AsSingle();

        Container.Bind<AudioSource>()
            .FromInstance(audioSource)
            .AsSingle();

        Container.Bind<AudioClip>()
            .WithId("ShootClip")
            .FromInstance(shootClip);

        Container.Bind<AudioClip>()
            .WithId("HitClip")
            .FromInstance(hitClip);

        Container.Bind<PlayerShooter>()
            .FromInstance(playerShooter)
            .AsSingle()
            .NonLazy();

        Container.BindFactory<
                BulletSpawnData,
                Bullet,
                Bullet.Factory>()
            .FromPoolableMemoryPool<
                BulletSpawnData,
                Bullet,
                Bullet.Pool>(pool =>
            {
                pool.WithInitialSize(initialBulletCount)
                    .FromComponentInNewPrefab(bulletPrefab)
                    .UnderTransform(bulletPoolRoot);
            });

        Container.Bind<Transform>()
            .WithId("PlayerTransform")
            .FromInstance(playerTransform)
            .AsCached();

        Container.Bind<GameObject>()
            .WithId("ObstaclePrefab")
            .FromInstance(obstaclePrefab)
            .AsSingle();

        Container.Bind<Transform>()
            .WithId("ObstaclesRoot")
            .FromInstance(obstaclesRoot)
            .AsCached();

        Container.Bind<ObstacleSpawner>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();
    }
}
