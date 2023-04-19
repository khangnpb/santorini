using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class AutoHostClient : MonoBehaviour
{
    
    [SerializeField] NetworkManager networkManager;

    public void JoinLocal()
    {
        string hostName = GameObject.Find("HostName").GetComponent<TMP_InputField>().text;
        networkManager.networkAddress = hostName;
        networkManager.StartClient();
    }

    //create function to send message to server
    public void Send(string message)
    {
        //networkManager.lateUpdate();
    }
    //create function to receive message from server
    public void Receive(string message)
    {
        //print message
        Debug.Log(message);//
    }
}
