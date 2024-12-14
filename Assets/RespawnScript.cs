using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    public float threshold; // The y value at which the player will respawn

    private void FixedUpdate()
    {
        if(transform.position.y < threshold)
        {
            transform.position = new Vector3(0.0f, 1.789f, 0.0f);
            GameManager.health--;
        }
    }

}
