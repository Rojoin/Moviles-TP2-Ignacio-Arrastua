using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;

public class GooglePlayController : MonoBehaviour
{
    // Start is called before the first frame update
   

    public void Start()
    {
        SignIn();
    }

    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    private void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            // Continue with Play Games Services
            string userName = PlayGamesPlatform.Instance.GetUserDisplayName();
            string id = PlayGamesPlatform.Instance.GetUserId();
            string imageUrl = PlayGamesPlatform.Instance.GetUserImageUrl();

            Debug.Log("Sucessfully login:" + userName);
        }
        else
        {
            Debug.Log("Sign in Failed!");
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }

    public void ManualSignIn()
    {
        PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
    }
}