using UnityEngine;

public class DestructibleObstacle : MonoBehaviour
{
    [SerializeField] private GameObject debrisPrefab;
    [SerializeField] private int debrisCount = 6;

    private bool isDestroyed;

    public void DestroyObstacle()
    {
        if (isDestroyed)
            return;

        isDestroyed = true;

        if (debrisPrefab != null)
        {
            for (int i = 0; i < debrisCount; i++)
            {
                Vector2 randomOffset = Random.insideUnitCircle * 0.3f;

                GameObject debris = Instantiate(
                    debrisPrefab,
                    (Vector2)transform.position + randomOffset,
                    Quaternion.identity
                    );

                Rigidbody2D debrisRb = debris.GetComponent<Rigidbody2D>();

                if (debrisRb != null)
                {
                    Vector2 force = Random.insideUnitCircle.normalized;
                    debrisRb.AddForce(force * Random.Range(1f, 3f),
                        ForceMode2D.Impulse);
                }
            }
        }

        Destroy(gameObject);
    }
}
