using System.Threading.Tasks;
using UnityEngine;
using Firebase.Database;

namespace Com.RedicalGames.Filar
{

    public class NetworkManager : AppMonoBaseClass
    {
        #region Static

        private static NetworkManager _instance;

        public static NetworkManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<NetworkManager>();

                return _instance;
            }
        }

        #endregion

        #region Components

        public NetworkReachability status;

        bool serverConnected = false;

        public bool Connected { get { return (status == NetworkReachability.NotReachable) ? false : true; } private set { } }

        #endregion

        #region Main

        #region Server Connection

        public async Task<AppData.Callback> ServerConnected()
        {
            AppData.Callback callbackResults = new AppData.Callback();

            FirebaseDatabase.DefaultInstance.GoOnline();

            await Task.Delay(100);

            DatabaseReference connectionRefererence = FirebaseDatabase.DefaultInstance.GetReference(".info/connected");

            connectionRefererence.ValueChanged += OnConnectionStatusChangedEvent;

            while (!serverConnected)
                await Task.Yield();

            return callbackResults;
        }

        private void OnConnectionStatusChangedEvent(object sender, ValueChangedEventArgs connectionStatus) => serverConnected = (bool)connectionStatus.Snapshot.Value;

        #endregion

        #region Networking

        public async Task<AppData.Callback> CheckConnectionStatus()
        {
            AppData.Callback callbackResults = new AppData.Callback();

            float timeOut = DefaultTimeOut();  

            status = Application.internetReachability;

            await Task.Delay(NetworkConnectionDelay());

            while (status == NetworkReachability.NotReachable || timeOut > 0.0f)
            {
                timeOut -= 1 * Time.deltaTime; ;

                if (status != NetworkReachability.NotReachable && timeOut > 0 || timeOut <= 0)
                    break;
            }

            string result = (status != NetworkReachability.NotReachable) ? "Network Connection Available." : "Network Connection Not Available.";
            callbackResults.SetResults(result, (status != NetworkReachability.NotReachable) ? AppData.LogInfoChannel.Success : AppData.LogInfoChannel.Error);

            return callbackResults;
        }

        int NetworkConnectionDelay()
        {
            var sceneAssetsManagerCallbackResults = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "Scene Assets Manager Instance Is Not Yet initialized.");

            if (sceneAssetsManagerCallbackResults.Success())
            {
                var sceneAssetsManager = sceneAssetsManagerCallbackResults.data;
                return AppData.Helpers.ConvertSecondsFromFloatToMillisecondsInt(sceneAssetsManager.GetDefaultExecutionValue(AppData.RuntimeExecution.NetworkInitializationDefaultDuration).value);
            }
            else
                Log(sceneAssetsManagerCallbackResults.ResultCode, sceneAssetsManagerCallbackResults.Result, this);

            return 0;
        }

        #endregion

        #region Time

        float DefaultTimeOut()
        {
            var sceneAssetsManagerCallbackResults = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "Scene Assets Manager Instance Is Not Yet initialized.");

            if (sceneAssetsManagerCallbackResults.Success())
            {
                var sceneAssetsManager = sceneAssetsManagerCallbackResults.data;
                return AppData.Helpers.ConvertSecondsFromFloatToMillisecondsInt(sceneAssetsManager.GetDefaultExecutionValue(AppData.RuntimeExecution.DefaultAppTimeout).value);
            }
            else
                Log(sceneAssetsManagerCallbackResults.ResultCode, sceneAssetsManagerCallbackResults.Result, this);

            return 0;
        }

        #endregion

        #endregion
    }
}