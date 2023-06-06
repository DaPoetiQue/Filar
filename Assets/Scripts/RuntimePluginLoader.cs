using UnityEngine;
using UnityEngine.Android;

public class RuntimePluginLoader : MonoBehaviour
{
    #region Components

    AndroidJavaClass unityClass;
    AndroidJavaObject unityActivity;
    AndroidJavaObject pluginInstance;

    const string pluginName = "com.redicalgames.androidfilemanagerlibrary.AndroidFileManager"; 


    #endregion

    #region Unity Callbacks

    private void Start()
    {
        RequestAndroidPermission();

        InitializePlugin(pluginName);
    }

    #endregion

    #region Main

    void RequestAndroidPermission()
    {
        #if UNITY_ANDROID

        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            Permission.RequestUserPermission(Permission.ExternalStorageRead);

        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);

        #endif
    }

    void InitializePlugin(string pluginName)
    {
        unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityClass.CallStatic<AndroidJavaObject>("currentActivity");
        pluginInstance = new AndroidJavaObject(pluginName);

        if(pluginInstance == null)
        {
            Debug.Log("--> Failed to initialize Android plugin from Unity3D.");
            return;
        }
        else
        {
            pluginInstance.CallStatic("InitializeUnityPlugin", unityActivity);
        }
    }

    public void ShowAndroidToast()
    {
        if(pluginInstance != null)
        {
            pluginInstance.Call("Toast", "Yey Wena!. It WORKS. hay, What a wanda.");
        }
        else
        {
            Debug.LogWarning($"Failed to load plugin : {pluginName} from Unity3D.");
            return;
        }
    }

    #endregion
}
