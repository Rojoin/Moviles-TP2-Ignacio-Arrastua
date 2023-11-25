using System;
using System.Collections.Generic;
using System.Net.Sockets;
using CustomSceneSwitcher.Switcher;
using CustomSceneSwitcher.Switcher.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;


public class TestPluginLog : MonoBehaviour
{
    private const string packageName = "com.rojoin.UnityLogger";
    private const string className = packageName + ".Logger";
    [SerializeField] private TextMeshProUGUI textBox;
    public GameObject textMeshProPrefab;
    public Transform contentParent;
    private List<GameObject> logsList = new List<GameObject>();
    [SerializeField] private Button returnButton;
    public SceneChangeData MainMenu;
#if UNITY_ANDROID
    private AndroidJavaClass PluginClass;
    private AndroidJavaClass unityPlayer;
    private AndroidJavaObject pluginInstance;
    private AndroidJavaObject unityActivity;

    private const string permission = "android.permission.WRITE_EXTERNAL_STORAGE";

#endif

    private void Start()
    {
        returnButton.onClick.AddListener(GoToMenu);
        if (Application.platform == RuntimePlatform.Android)
        {
            PluginClass = new AndroidJavaClass(className);
            pluginInstance = MobileLogger.Instance.PluginInstance;
        }
    }

    private void GoToMenu()
    {
        SceneSwitcher.ChangeScene(MainMenu);
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

    public void DeleteLogs()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            pluginInstance.Call("ShowAlert");
        }
    }

    public void ReadLogs()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            string logs = pluginInstance.Call<string>("readFile");
            string[] lines = logs.Split(new[] { "\n", "\r\n" }, System.StringSplitOptions.None);

            if (logsList.Capacity > 0)
            {
                foreach (GameObject VARIABLE in logsList)
                {
                    Destroy(VARIABLE);
                }
            }
            logsList.Clear();
            foreach (string line in lines)
            {
                GameObject textObject = Instantiate(textMeshProPrefab, contentParent);
                TextMeshProUGUI textComponent = textObject.GetComponent<TextMeshProUGUI>();
                textComponent.text = line;
                logsList.Add(textObject);
            }
        }
    }

    private void OnDestroy()
    {

        returnButton.onClick.RemoveListener(GoToMenu);
    }

    public void RunPlugin()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            string text = "Works";
            textBox.text = text;
            Debug.Log(text);
        }
    }

    public void TestWarning()
    {
        string text = "This is a Warning";
        textBox.text = text;
        Debug.LogWarning(text);
    } 
    public void TestLog()
    {
        string text = "This is a Log";
        textBox.text = text;
        Debug.Log(text);
    }
    public void TestError()
    {
        string text = "This is a Error";
        textBox.text = text;
        Debug.LogError(text);
    }
  
}