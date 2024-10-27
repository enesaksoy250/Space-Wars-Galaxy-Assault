using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class ServerManager : MonoBehaviourPunCallbacks
{

  
    private TMP_InputField createRoomInput,joinRoomInput;
    private int shipIndex;
    private bool isWaitingTextDisabled = false;
    CountdownScript countdown;
 
    public static ServerManager instance;

    private void Awake()
    {
       
        SceneControl();

    }

    void Start()
    {
  
        ConnectToMaster();
        InvokeRepeating(nameof(ConnectControl), 1, 1);
        InvokeRepeating(nameof(CheckInternetConnection), 3, 1);

    }



    public void CreateRoom()
    {

        PhotonNetwork.JoinLobby();

    }

    public void JoinRoom()
    {
        string roomName = joinRoomInput.text;
        if (!string.IsNullOrEmpty(roomName))
        {
            PhotonNetwork.JoinRoom(roomName);
        }
        else
        {
            StartCoroutine(MainMenuPanelManager.instance.OpenAndClosePanel("ErrorPanel"));
        }
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("Rastgele odaya girilemedi");
        StartCoroutine(MainMenuPanelManager.instance.OpenAndClosePanel("ErrorPanel"));
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("Create room failed: " + message);
        StartCoroutine(MainMenuPanelManager.instance.OpenAndClosePanel("ErrorPanel"));

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("Join room failed: " + message);
        StartCoroutine(MainMenuPanelManager.instance.OpenAndClosePanel("ErrorPanel"));
    }

    public override void OnJoinedRoom()
    {

        PhotonNetwork.LoadLevel(13);
        print("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        Invoke(nameof(CreatePlayer), 2);
        InvokeRepeating(nameof(CheckInformation), 0, .1f);
        InvokeRepeating(nameof(PlayerListControl), 0, .1f);

    }

    void CreatePlayer()
    {

        shipIndex = PlayerPrefs.GetInt("SelectedSpaceship");

        if (shipIndex == 0)
            shipIndex = 1;

        GameObject player = PhotonNetwork.Instantiate("Player" + shipIndex, Vector3.zero, Quaternion.identity);
        int actorNumber = player.GetComponent<PhotonView>().Owner.ActorNumber;

        player.GetComponent<PlayerOnlineSettings>().GetUsername(userName =>
        {         
            player.GetComponent<PhotonView>().Owner.NickName = userName;
        });

    }

    public override void OnConnectedToMaster()
    {

        print("Connected to master!");
      

    }

    public override void OnDisconnected(DisconnectCause cause)
    {

        print("Disconnected!");
      

    }

    public void ConnectToMaster()
    {

        if (!PhotonNetwork.IsConnected)
        {

            PhotonNetwork.ConnectUsingSettings();

        }

    }

    public override void OnJoinedLobby()
    {
        print("Joined lobby");

        string roomName = createRoomInput.text;
        if (!string.IsNullOrEmpty(roomName))
        {
            PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true });
        }
        else
        {
            StartCoroutine(MainMenuPanelManager.instance.OpenAndClosePanel("ErrorPanel"));
        }

    }

    public void SetInputField()
    {

        createRoomInput = GameObject.Find("CreateRoomIF").GetComponent<TMP_InputField>();
        joinRoomInput = GameObject.Find("JoinTheRoomIF").GetComponent<TMP_InputField>();

    }

    public void LeaveRoom()
    {

        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();

    }

    public override void OnLeftRoom()
    {
        print("Leaved room!");
        SceneManager.LoadScene(0);
        Destroy(gameObject, .5f);

    }

    private void CheckInformation()
    {
      

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {

            Invoke(nameof(GetUsernames), 2);

            if (!isWaitingTextDisabled)
            {
                GameObject.FindWithTag("WaitingText").GetComponent<TextMeshProUGUI>().enabled = false;
                GameObject.FindWithTag("InfoText").GetComponent<TextMeshProUGUI>().enabled = true;
                Invoke(nameof(CloseInfoText), 6);
                isWaitingTextDisabled = true;
            }

            CancelInvoke(nameof(CheckInformation));

        }

        else
        {

            string username1 = PhotonNetwork.PlayerList[0].NickName;
           
            OnlineGameManager.instance.SetUsernameText(username1,"........");

            if (!isWaitingTextDisabled)
            {
                GameObject.FindWithTag("WaitingText").GetComponent<TextMeshProUGUI>().enabled = true;
            }

            else
            {

                CancelInvoke(nameof(CheckInformation));
                int actorNumber = PhotonNetwork.PlayerList[0].ActorNumber;
             
                OnlineGameManager.instance.player1Start = false;
                OnlineGameManager.instance.player2Start = false;
                OnlineGameManager.instance.LoadWinPanel(actorNumber);           
         
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                Invoke(nameof(LeaveRoom), 10);


            }
        }

    }

    private void GetUsernames()
    {

        string username1 = PhotonNetwork.PlayerList[0].NickName;
        string username2 = PhotonNetwork.PlayerList[1].NickName;

        OnlineGameManager.instance.SetUsernameText(username1, username2);

    }

    private void CloseInfoText()
    {

        GameObject.FindWithTag("InfoText").GetComponent<TextMeshProUGUI>().enabled = false;

    }

    public void UpdateHealthText(float newHealth, int targetPlayer)
    {

        switch (targetPlayer)
        {

            case 1:
                GameObject.FindWithTag("HealthText0").GetComponent<TextMeshProUGUI>().text = newHealth.ToString();
                break;

            case 2:
                GameObject.FindWithTag("HealthText1").GetComponent<TextMeshProUGUI>().text = newHealth.ToString();
                break;


        }


    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        InvokeRepeating(nameof(CheckInformation), 0, .5f);
    }

    private void PlayerListControl()
    {

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {           

             countdown = FindObjectOfType<CountdownScript>();
             countdown.StartCountdown();               
             CancelInvoke(nameof(PlayerListControl));

        }

    }

    public static int GetPlayerListLength()
    {

        return PhotonNetwork.CurrentRoom.PlayerCount;

    }

    private void ConnectControl()
    {

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {

            if (PhotonNetwork.IsConnected)
            {

                GameObject.FindWithTag("PlayOnlineButton").GetComponent<Button>().interactable = true;

            }

            else
            {

                GameObject.FindWithTag("PlayOnlineButton").GetComponent<Button>().interactable = false;

            }


        }


    }

    private void SceneControl()
    {

        int buildIndex = SceneManager.GetActiveScene().buildIndex;

        if (buildIndex != 0 && buildIndex != 13)
        {

            Destroy(gameObject);

        }

        else
        {

            instance = this;
            DontDestroyOnLoad(gameObject);

        }

    }

    private void CheckInternetConnection()
    {

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {

            bool internetConnect = NetworkManager.IsInternetAvailable();
            GameObject.FindWithTag("ProfilButton").GetComponent<Button>().interactable = internetConnect;
            GameObject.FindWithTag("LButton").GetComponent<Button>().interactable = internetConnect;

        }
      
    }


    public void Disconnect()
    {

        PhotonNetwork.Disconnect();

    }

}
