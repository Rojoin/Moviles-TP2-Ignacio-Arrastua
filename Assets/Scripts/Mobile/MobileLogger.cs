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
    public AndroidJavaObject PluginInstance { get; private set; }
    private AndroidJavaObject unityActivity;

    private const string permission = "android.permission.WRITE_EXTERNAL_STORAGE";

#endif

    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value ;
            PluginInstance = new AndroidJavaObject(className);

            unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            PluginInstance.CallStatic("initialize", unityActivity);


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

            PluginInstance.Call("CreateAlert");
        }
    }

    private void SendLogToAndroid(string logString, string stackTrace, LogType type)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            switch (type)
            {
                case LogType.Error:
                    PluginInstance.Call("SendLog", logString, 2);
                    break;
                case LogType.Assert:
                    PluginInstance.Call("SendLog", logString);
                    break;
                case LogType.Warning:
                    PluginInstance.Call("SendLog", logString, 1);
                    break;
                case LogType.Log:
                    PluginInstance.Call("SendLog", logString, 0);
                    break;
                case LogType.Exception:
                    PluginInstance.Call("SendLog", logString, 3);
                    break;
            }
        }
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= SendLogToAndroid;
    }
}