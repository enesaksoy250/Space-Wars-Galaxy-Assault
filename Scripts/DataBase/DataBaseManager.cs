using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using System.IO;
using Firebase.Auth;
using Firebase;
using Firebase.Platform;


public class DataBaseManager : MonoBehaviour
{

    private TMP_InputField usernameField;
    private TMP_InputField emailField;

    private DatabaseReference databaseReference;
  
    private string userID;
    private string userName;


    public static DataBaseManager instance;

    private void Awake()
    {

        if (instance == null)
        {

            instance = this;
            DontDestroyOnLoad(this);

        }

        else
        {

            Destroy(gameObject);

        }

        

    }


    void Start()
    {
      
        LoginControl();
        Invoke(nameof(SetID), .5f);

    }

    private void SetID()
    {

        userID = SystemInfo.deviceUniqueIdentifier;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

    }

    public string GetUserId()
    {

        return userID;

    }

    private void LoginControl()
    {

        if (!PlayerPrefs.HasKey("Login"))
        {        

            MainMenuPanelManager.instance.LoadPanel("ChooseLanguagePanel"); 
       
        }

        else
        {

            Invoke(nameof(GetInfoFromFirebase), 1);
            Invoke(nameof(PrintTopTenUsers), 1);

        }

    }
  
    public void Save()
    {

      
        usernameField = GameObject.FindWithTag("UsernameIF").GetComponent<TMP_InputField>();
        emailField = GameObject.FindWithTag("EmailIF").GetComponent<TMP_InputField>();
    
        string username = usernameField.text;
        string email = emailField.text;

        if (string.IsNullOrEmpty(username))
        {
            Debug.LogError("Kullan�c� ad� bo� b�rak�lamaz.");
            StartCoroutine(MainMenuPanelManager.instance.OpenAndClosePanel("EmptyErrorPanel"));
            return;
        }

        if (string.IsNullOrEmpty(email))
        {
            Debug.LogError("email bo� b�rak�lamaz.");
            StartCoroutine(MainMenuPanelManager.instance.OpenAndClosePanel("EmptyErrorPanel"));
            return;
        }

        CheckIfUsernameExists(username, (exists) => {
            if (exists)
            {
                StartCoroutine(MainMenuPanelManager.instance.OpenAndClosePanel("UsernameErrorPanel"));
                Debug.LogError("Kullan�c� ad� daha �nce al�nd�");
            }
            else
            {
                
                WriteNewUser(username, email);
            }
        });


    }

    void CheckIfUsernameExists(string username, Action<bool> callback)
    {
          databaseReference.Child("users").OrderByChild("username").EqualTo(username)
            .GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted)
                {
                    Debug.LogError("Kullan�c� ad� kontrol edilirken hata olu�tu: " + task.Exception);
                    callback(false); 
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    callback(snapshot.Exists); 
                }
            });
    }


    void WriteNewUser(string username, string email)
    {
        User user = new User(username, email,0,0,0,0);
        string json = JsonUtility.ToJson(user);

        
        databaseReference.Child("users").Child(userID).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log("Kay�t Ba�ar�l�");
                MainMenuPanelManager.instance.ClosePanel("MainRegistrationPanel");
                PlayerPrefs.SetInt("Login", 1);
                Invoke(nameof(GetInfoFromFirebase), 1);
                Invoke(nameof(PrintTopTenUsers), 1);
            }
            else
            {
                Debug.LogError("Kay�t hatas�");
            }
        });
    }

    

    public void GetUserDataFromFirebase(string key, Action<object> onSuccess)
    {
       
        databaseReference.Child("users").Child(userID).Child(key).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"Veritaban�ndan {key} al�n�rken bir hata olu�tu: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    onSuccess(snapshot.Value);
                }
                else
                {
                    Debug.LogError($"{key} de�eri al�namad�!");
                }
            }
        });
    }

    public void GetInfoFromFirebase()
    {
        var dataKeys = new Dictionary<string, Action<object>>
      {
        { "username", value => UserFirebaseInformation.instance.userName = value.ToString() },
        { "gameTime", value => UserFirebaseInformation.instance.gameTime = FormatGameTime(int.Parse(value.ToString())) },
        { "totalLoss", value => UserFirebaseInformation.instance.totalLoss = int.Parse(value.ToString()) },
        { "totalDestruction", value => UserFirebaseInformation.instance.totalDestruction = int.Parse(value.ToString()) }
      };

        foreach (var entry in dataKeys)
        {
            GetUserDataFromFirebase(entry.Key, entry.Value);
        }
    }

    public void GetUsername(Action<string> onUsernameReceived)
    {

        GetUserDataFromFirebase("username", value =>
        {

            userName = value.ToString();
            onUsernameReceived?.Invoke(userName);
            UserFirebaseInformation.instance.userName = userName;

        });



    }

    private string FormatGameTime(int gameTime)
    {
        if (gameTime < 60)
        {
            return gameTime + "s";
        }
        else if (gameTime < 3600)
        {
            int minutes = gameTime / 60;
            int seconds = gameTime % 60;
            return string.Format("{0}m {1}s", minutes, seconds);
        }
        else
        {
            int hours = gameTime / 3600;
            int minutes = (gameTime % 3600) / 60;
            int seconds = gameTime % 60;
            return string.Format("{0}h {1}m {2}s", hours, minutes, seconds);
        }
    }

    public void UpdateFirebaseInfo(string statName, int incrementValue)
    {
        databaseReference.Child("users").Child(userID).Child(statName).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError($"Veritaban�ndan {statName} al�n�rken bir hata olu�tu: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int currentValue = snapshot.Exists ? int.Parse(snapshot.Value.ToString()) : 0;
                int newValue = currentValue + incrementValue;
                databaseReference.Child("users").Child(userID).Child(statName).SetValueAsync(newValue);
                UpdateUserInfo(statName, newValue);
            }
        });
    }

    public void PrintTopTenUsers()
    {
        databaseReference.Child("users").OrderByChild("onlineWins").LimitToLast(10).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to retrieve leaderboard data.");
                PrintTopTenUsers();
                return;
            }

            DataSnapshot snapshot = task.Result;
            List<User> topUsers = new List<User>();

            foreach (var childSnapshot in snapshot.Children)
            {
                User user = JsonUtility.FromJson<User>(childSnapshot.GetRawJsonValue());
                topUsers.Add(user);
            }

            topUsers.Sort((x, y) => y.onlineWins.CompareTo(x.onlineWins));

            for (int i = 0; i < topUsers.Count; i++)
            {

                string userName = topUsers[i].username;
                int wins = topUsers[i].onlineWins;

                UserFirebaseInformation.instance.usernameArray[i] = userName;
                UserFirebaseInformation.instance.winsArray[i] = wins;

            }

        });
    }

    private void UpdateUserInfo(string statName, int newValue)
    {

        switch (statName)
        {

            case "totalLoss":
                UserFirebaseInformation.instance.totalLoss = newValue;
                break;

            case "totalDestruction":
                UserFirebaseInformation.instance.totalDestruction = newValue;
                break;

            case "gameTime":
                string formattedGameTime = FormatGameTime(newValue);
                UserFirebaseInformation.instance.gameTime = formattedGameTime;
                break;

        }



    }

}
