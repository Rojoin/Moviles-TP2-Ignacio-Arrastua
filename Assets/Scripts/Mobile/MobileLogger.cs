using UnityEngine;
using UnityEngine.Android;

public class MobileLogger : MonoBehaviour
{
    public static MobileLogger Instance { get; private set; }
    private void Awake() 
    { 
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
            DontDestroyOnLoad(this);
        } 
    }
    
#if UNITY_ANDROID
    private const string packageName = "com.rojoin.UnityLogger";
    private const string className = packageName + ".Logger";
    private AndroidJavaClass unityPlayer;
    private AndroidJavaObject pluginInstance;
    private AndroidJavaObject unityActivity;

    private const string permission = "android.permission.WRITE_EXTERNAL_STORAGE";

#endif

    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            pluginInstance = new AndroidJavaObject(className);

            unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            pluginInstance.CallStatic("initialize", unityActivity);


            Application.logMessageReceived += SendLogToAndroid;
            if (Permission.HasUserAuthorizedPermission(permission))
            {
                Debug.Log("Permission is already granted.");
            }
            else
            {
                // Request permission
                Permission.RequestUserPermission(permission);
            }

            pluginInstance.Call("CreateAlert");
        }
    }

    private void SendLogToAndroid(string logString, string stackTrace, LogType type)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            switch (type)
            {
                case LogType.Error:
                    pluginInstance.Call("SendLog", logString, 2);
                    break;
                case LogType.Assert:
                    pluginInstance.Call("SendLog", logString);
                    break;
                case LogType.Warning:
                    pluginInstance.Call("SendLog", logString, 1);
                    break;
                case LogType.Log:
                    pluginInstance.Call("SendLog", logString, 0);
                    break;
                case LogType.Exception:
                    pluginInstance.Call("SendLog", logString, 3);
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= SendLogToAndroid;
    }
}