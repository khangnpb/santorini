// using System;
// using System.Collections;
// using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;


public class NetworkManager : MonoBehaviourPunCallbacks 
{
    [SerializeField] private MainMenu mainMenuManager;
    string nickName = "";


    private void Update()
    {
        string clientState = "";
        if(PhotonNetwork.NetworkClientState != null)
            clientState = PhotonNetwork.NetworkClientState.ToString();
        if (clientState == "ConnectingToMasterServer")
        {
            clientState = "Connecting to server...";
        }
        else if (clientState == "ConnectedToMasterServer")
        {
            clientState = "Connected to server...";
        }
        else if (clientState == "DisconnectingFromMasterServer")
        {
            clientState = "Disconnecting from server...";
        }
        else if(clientState == "ConnectingToGameServer")
        {
            clientState = "Connecting To Game Server...";
        }
        else if(clientState == "ConnectedToGameServer")
        {
            clientState = "Connected to Game server";
        }
        else if(clientState == "FailedToConnectToMasterServer")
        {
             clientState = "Failed to connect to server";
        }
        else if(clientState == "ConnectionFailed")
        {
            clientState = "Connection Failed";
        }
        mainMenuManager.SetConnectionStatusText(clientState);
    }

    public void Connect()
    {
        PhotonNetwork.NickName = nickName;
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else 
        {
            PhotonNetwork.ConnectUsingSettings();
        } 
    }

    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log($"Connected to serrver. Lookingh for random room");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"Joining random room failed because of {message}. Creating a new one");
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        
        Debug.Log($"Player {PhotonNetwork.LocalPlayer.ActorNumber} joined the room");
        Debug.Log($"Nguoi choi {PhotonNetwork.LocalPlayer.NickName} joined the room");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Called when another player joins the room
        Debug.Log("Player " + newPlayer.NickName + " joined room " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log($"Nguoi choi {newPlayer.NickName} joined the room");
    }
    public void SetNickName()
    {
        // GetComponentInChildren
        nickName = mainMenuManager.getNickName();
    }
    #endregion

    
    
}
