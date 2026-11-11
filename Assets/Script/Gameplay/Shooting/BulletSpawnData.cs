using UnityEngine;

public class BulletSpawnData
{
    public TargetData TargetData { get; }
    public Transform TargetTransform { get; }

    public BulletSpawnData(
        TargetData targetData,
        Transform targetTransform)
    {
        TargetData = targetData;
        TargetTransform = targetTransform;
    }
}
