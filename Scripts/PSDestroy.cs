using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Unity.VisualScripting;


public class PSDestroy : MonoBehaviour {


	PhotonView pw;
	
	void Start () {

		int sceneIndex = SceneManager.GetActiveScene().buildIndex;

		if(sceneIndex != 13)
		{

            Destroy(gameObject, 1);

        }

		else
		{

			pw = GetComponent<PhotonView>();
			Invoke(nameof(DestroyPS),1);

		}
		
	}
	
	private void DestroyPS()
	{
        if (pw != null && pw.IsMine)
            PhotonNetwork.Destroy(gameObject);
	}

}
