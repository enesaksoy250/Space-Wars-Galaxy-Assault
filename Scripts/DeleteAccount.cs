using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteAccount : MonoBehaviour
{
    public const string url = "https://docs.google.com/forms/d/e/1FAIpQLSdWJLnV0YILv9shRtuLjgpnSTwTCOTmRLBfm4q_x-1LO49lGQ/viewform?usp=sf_link";

    public void OpenLink()
    {
        Application.OpenURL(url);
    }
}
