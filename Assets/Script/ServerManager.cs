using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Photon.Chat;
using Photon.Chat.Demo;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ServerManager : Singleton<ServerManager>
{
    private string selectedChannelName;
    private byte maxPlayersPerRoom = 0;
    public bool isConnecting;
    
    public void Init()
    {
      
        Debug.Log("Server manager init");
    }
    
    private void Start()
    {
        //Initialize();
        isConnecting = false;
    }
    
    public void Initialize()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Connect();
    }

    //------------------------------ region divider-----------------------
    
    #region room

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Connected...");
            //PhotonNetwork.JoinRandomRoom();
            PhotonNetwork.JoinOrCreateRoom("test", new RoomOptions { MaxPlayers = this.maxPlayersPerRoom }, null, null);
        }
        else
        {
            Debug.Log("Connecting...");
            PhotonNetwork.ConnectUsingSettings();
        }

        StartCoroutine(StartLoadingMonitoring());
    }
    
    private IEnumerator StartLoadingMonitoring()
    {
        while (PhotonNetwork._AsyncLevelLoadingOperation == null)
        {
            Debug.Log("LoadScene process didn't start");
            yield return new WaitForFixedUpdate();
        }

        while (PhotonNetwork._AsyncLevelLoadingOperation.progress < 0.99)
        {
            yield return new WaitForFixedUpdate();
            if(PhotonNetwork._AsyncLevelLoadingOperation!=null)
                Debug.Log("loading percent : " + PhotonNetwork._AsyncLevelLoadingOperation.progress * 100 + "%");//ReactCommunicator.Instance.SendLoadScenePercent(PhotonNetwork._AsyncLevelLoadingOperation.progress);
            else
                break;
        }
		
        StartCoroutine(StartConnectionMonitoring());

        yield return null;
    }
	
    public IEnumerator StartConnectionMonitoring()
    {
        //ProcessManager.Instance.SetRandomAvatarState();
        ProcessManager.Instance.Initialize("BasicAvatar");
        while (PhotonNetwork.IsConnected)
        {
            yield return new WaitForFixedUpdate();
        }

        isConnecting = false;
        Debug.Log("PhotonNetwork disconnected");
        ReactCommunicator.Instance.SendDisconnectSign();

        yield return null;
    }
    
    public override void OnConnectedToMaster()
    {
            Debug.Log("OnConnectedToMaster");
            //PhotonNetwork.JoinRandomRoom();
            PhotonNetwork.JoinOrCreateRoom("test", new RoomOptions { MaxPlayers = this.maxPlayersPerRoom }, null, null);
    }
    
    public override void OnJoinedRoom()
    {
        Debug.Log("<Color=Green>OnJoinedRoom</Color> with " + PhotonNetwork.CurrentRoom.PlayerCount + " Player(s)");
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");

        // #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.AutomaticallySyncScene to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("We load the 'Room for 1' ");
            
            PhotonNetwork.LoadLevel("CounselingRoom");
        }
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 새로운 사용자가 방에 입장했을 때 호출되는 메서드
        Debug.Log(newPlayer.NickName + " entered the room");
        StartCoroutine(SendAdjustNewCustom());
    }
    
    private IEnumerator SendAdjustNewCustom()
    {
        yield return new WaitForSeconds(2f);
        AdjustNewestCustom();
    }

    public void AdjustNewestCustom()
    {
        //ProcessManager.Instance.SetRandomAvatarState();
        if(!isConnecting) return;
        string data = JsonUtility.ToJson(ProcessManager.Instance.state);
        ProcessManager.Instance.player.GetComponent<PhotonView>().RPC("SetNewCustom", RpcTarget.AllBuffered, data,
            ProcessManager.Instance.player.GetComponent<PhotonView>().InstantiationId);
        //yield return null;
    }
    #endregion

  
    
}
