using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;

public class GooglePlayController : MonoBehaviour
{
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
            PlayGamesPlatform.Activate();
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("Google Play Games Services authenticated successfully");
                }
                else
                {
                    Debug.LogError("Failed to authenticate with Google Play Games Services");
                }
            });
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

    public static void UnlockAchievement(string achievementId)
    {
        // Unlock achievement
        Social.ReportProgress(achievementId, 100.0, (bool success) =>
        {
            if (success)
            {
                Debug.Log("Achievement unlocked: " + achievementId);
            }
            else
            {
                Debug.LogError("Failed to unlock achievement: " + achievementId);
            }
        });
    }
}