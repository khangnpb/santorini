using UnityEngine;

public class HandleMenu : MonoBehaviour
{
    public void loadScene(string SceneName)
    {
        //load scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
    }
}