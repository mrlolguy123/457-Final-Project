using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private bool hasHit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        if (other.CompareTag("Player"))
        {
            hasHit = true;
            GameManager.health--;
            Destroy(gameObject);
        }
    }
}
