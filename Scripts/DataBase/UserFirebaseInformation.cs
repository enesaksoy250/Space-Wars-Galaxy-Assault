using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserFirebaseInformation : MonoBehaviour
{

    public string userName { get; set; }
    public string gameTime { get; set; }
    public int totalDestruction { get; set; }
    public int totalLoss { get; set; }
    public string[] usernameArray { get; set; } = new string[10];
    public int[] winsArray { get; set; } = new int[10];

    public static UserFirebaseInformation instance;

    private void Awake()
    {
        
        if(instance == null)
        {

            instance = this;
            DontDestroyOnLoad(gameObject);

        }

        else
        {

            Destroy(gameObject);

        }

 
    }

 

}
