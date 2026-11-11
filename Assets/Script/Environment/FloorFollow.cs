using UnityEngine;

public class FloorFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float offsetY = 0f;

    private void LateUpdate()
    {
        if (player == null)
            return;

        Vector3 position = transform.position;
        position.y = player.position.y + offsetY;

        transform.position = position;
    }
}
