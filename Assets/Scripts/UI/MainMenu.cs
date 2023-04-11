using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Santorini1()
    {
        SceneManager.LoadScene("Santorini1");
    }
    public void Santorini2()
    {
        SceneManager.LoadScene("Santorini2");
    }
    public void Santorini3()
    {
        SceneManager.LoadScene("Santorini3");
    }
}
