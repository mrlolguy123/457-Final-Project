using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void RestartGame()
    {
        Time.timeScale = 1;
        GameManager.health = 3;
        ScoreManager.score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
