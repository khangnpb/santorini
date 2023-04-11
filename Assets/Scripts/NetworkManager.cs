// using System;
using ExitGames.Client.Photon;
// using System.Collections.Generic;

using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


public class NetworkManager : MonoBehaviourPunCallbacks 
{
    [Header("Dependencies")]
    [SerializeField] private MainMenu mainMenuManager;
    string nickName = "";

    [Header("Texts")]
    [SerializeField] private Text  waittingTheHost;
    [SerializeField] private Text yourName;
    [SerializeField] private Text yourOpponentName;


    [Header("Button")]
    [SerializeField] private Button HostConfirmButton;

    [Header("Toggle")]
    [SerializeField] private Toggle isBlue;
    [SerializeField] private Toggle isWhite;

    // [SerializeField] private PhotonView photonView;
        private PhotonView photonView;


    private const string TEAM_PROPERTY = "team";
    
    private bool isBlueTeamSelected = true;
    private bool isOwner = false;
    

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
        photonView = GetComponent<PhotonView>();
    }

    public void hideConfirmButton()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == PhotonNetwork.CurrentRoom.MasterClientId)
        {
            // Code to run if the current player is the host

            HostConfirmButton.gameObject.SetActive(true);
            waittingTheHost.gameObject.SetActive(false);
            isOwner = true;
            Debug.Log("You are host");
        }
        else
        {
            // Code to run if the current player is not the host
            isOwner = false;
            HostConfirmButton.gameObject.SetActive(false);
            waittingTheHost.gameObject.SetActive(true);
            isBlue.isOn = false;
            isWhite.isOn = true;
            isBlue.interactable = false;
            isWhite.interactable = false;
            Debug.Log("You are not the host");
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
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2; // Set the maximum number of players to 2
    }

    public override void OnJoinedRoom()
    {
        
        Debug.Log($"Player {PhotonNetwork.LocalPlayer.ActorNumber} joined the room");
        Debug.Log($"Nguoi choi {PhotonNetwork.LocalPlayer.NickName} joined the room");
        yourName.text = PhotonNetwork.LocalPlayer.NickName;
        if (PhotonNetwork.PlayerList.Length > 1)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (!player.IsLocal)
                {
                    yourOpponentName.text = player.NickName;
                    break;
                }
            }
            Debug.Log("Other player name: " + yourOpponentName.text);
        }
        else
        {
            Debug.Log("No other players in the room.");
        }
        mainMenuManager.ConnectScreen.gameObject.SetActive(false);
        mainMenuManager.TeamSelectionScreen.SetActive(true);
        hideConfirmButton();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Called when another player joins the room
        Debug.Log("Player " + newPlayer.NickName + " joined room " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log($"Nguoi choi {newPlayer.NickName} joined the room");
        yourOpponentName.text = newPlayer.NickName;
        
    }
    public void SetNickName()
    {
        // GetComponentInChildren
        nickName = mainMenuManager.getNickName();
    }


    private void PrepareTeamSelectionOptions()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            var firstPlayer = PhotonNetwork.CurrentRoom.GetPlayer(1);
        }
    }

    public void leaveRoom()
	{
		PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
	}

    public void OnBlueTeamSelected()
    {
        isBlueTeamSelected = true;
            // photonView.RPC("UpdateTeamSelection", RpcTarget.All, isBlueTeamSelected);
    }

    public void OnWhiteTeamSelected()
    {
        isBlueTeamSelected = false;
    }

    public void OnSubmitButtonClicked()
    {
        photonView.RPC("SetFinalTeamSelection", RpcTarget.All, isBlueTeamSelected);
    }

    [PunRPC]
    private void SetFinalTeamSelection(bool isBlueSelected)
    {

        if(isOwner)
        {
            isBlue.isOn = isBlueSelected;
            isWhite.isOn = !isBlueSelected;
        }
        else {
            isBlue.isOn = !isBlueSelected;
            isWhite.isOn = isBlueSelected;
            waittingTheHost.text = "Selected successfully! The game will be started...";
            waittingTheHost.color = Color.green;
        }

        // Disable the team toggles and submit button for both players
        isBlue.interactable = false;
        isWhite.interactable = false;
        HostConfirmButton.interactable = false;
    }

    #endregion
    
}
