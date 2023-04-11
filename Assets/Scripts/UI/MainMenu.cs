using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{


    [Header("Dependencies")]
	[SerializeField] private NetworkManager networkManager;


    [Header("Texts")]
	[SerializeField] private Text connectionStatus;

    [SerializeField] private TMP_InputField nickNameField;

    [Header("Screen Gameobjects")]
    [SerializeField] public GameObject MainScreen;
    [SerializeField] public GameObject ConnectScreen;
    [SerializeField] public GameObject TeamSelectionScreen;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnConnect()
    {
        networkManager.Connect();
        // networkManager.hideConfirmButton();
        
    }

    public void SetConnectionStatusText(string status)
	{
		connectionStatus.text = status;
	}

    public string getNickName()
    {
        return nickNameField.text;
    }
}
