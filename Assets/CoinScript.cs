using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public GameObject coinObj;
    public float rotationSpeed = 100f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void generateCoin()
    {
        Vector3 randomPosition = new Vector3(Random.Range(-10, 10), 1, Random.Range(-10, 10));
        GameObject newCoin = Instantiate(coinObj, randomPosition, Quaternion.identity);
        newCoin.SetActive(true);
        CoinScript coinScript = newCoin.GetComponent<CoinScript>();
        coinScript.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            ScoreManager.score++;
            generateCoin();
        }
    }
}
