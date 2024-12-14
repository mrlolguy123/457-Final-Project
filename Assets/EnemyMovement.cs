using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent navMeshAgent;

    [SerializeField] private float timer = 5f;
    private float bulletTime;

    public GameObject enemyBullet;
    public Transform spawnPoint;
    public float enemySpeed = 10f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        primitiveFollow();
        //predictiveFollow();
    }

    void primitiveFollow()
    {
        if (player != null)
        {
            navMeshAgent.SetDestination(player.position);
        }
        ShootAtPlayer();
    }

    void predictiveFollow()
    {
        if (player != null)
        {
            //Vector3 playerPos = player.position;
            //Vector3 playerVelocity = ???
            //Vector3 playerFuturePos = playerPos + playerVelocity * Time.deltaTime;

            //navMeshAgent.SetDestination(playerFuturePos);
        }
        ShootAtPlayer();
    }

    void ShootAtPlayer()
    {
        bulletTime -= Time.deltaTime;

        if (bulletTime > 0) return;

        bulletTime = timer;

        // Instantiate the bullet at the spawn point
        GameObject bulletObj = Instantiate(enemyBullet, spawnPoint.position, spawnPoint.rotation);
        bulletObj.SetActive(true);

        // Apply forward force to the bullet
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
        if (bulletRig != null)
        {
            // Apply force based on spawnPoint's forward direction
            bulletRig.AddForce(spawnPoint.forward * enemySpeed, ForceMode.Impulse);
        }
        // Make bullet Is Trigger
        bulletObj.GetComponent<Collider>().isTrigger = true;

        // Destroy bullet after 5 seconds
        Destroy(bulletObj, 1f);
    }
}
