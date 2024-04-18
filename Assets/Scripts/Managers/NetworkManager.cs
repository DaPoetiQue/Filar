using System.Threading.Tasks;
using UnityEngine;
using Firebase.Database;

namespace Com.RedicalGames.Filar
{
    public class NetworkManager : AppData.SingletonBaseComponent<NetworkManager>
    {
        #region Components

        public NetworkReachability status;

        bool serverConnected = false;

        public bool Connected { get { return (status == NetworkReachability.NotReachable) ? false : true; } private set { } }

        #endregion

        #region Main
        protected override void Init()
        {
           
        }

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

            if(serverConnected)
            {
                callbackResults.result = ".info/connected Successfully.";
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = ".info/connection Failed.";
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            return callbackResults;
        }

        private void OnConnectionStatusChangedEvent(object sender, ValueChangedEventArgs connectionStatus) => serverConnected = (bool)connectionStatus.Snapshot.Value;

        #endregion

        #region Networking

        public async Task<AppData.Callback> CheckConnectionStatus(bool blurScreen = false)
        {
            AppData.Callback callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(SurfacingManager.Instance, "Surfacing Manager", "Check Connection Status Failed - Surfacing Manager Instance Is Not Yet Initialized - Invalid Operation."));

            if (callbackResults.Success())
            {
                var surfacingManagerInstance = AppData.Helpers.GetAppComponentValid(SurfacingManager.Instance, "Surfacing manager").GetData();

                var loadingWidgetConfigDataPacket = new AppData.SceneConfigDataPacket();

                loadingWidgetConfigDataPacket.SetReferencedScreenType(AppData.ScreenType.LoadingScreen);
                loadingWidgetConfigDataPacket.SetReferencedWidgetType(AppData.WidgetType.LoadingWidget);
                loadingWidgetConfigDataPacket.SetReferencedUIScreenPlacementType(AppData.ScreenUIPlacementType.ForeGround);
                loadingWidgetConfigDataPacket.SetScreenBlurState(blurScreen);

                callbackResults.SetResult(surfacingManagerInstance.SurfaceWidget(loadingWidgetConfigDataPacket));

                if (callbackResults.Success())
                {
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

                    if (callbackResults.Success())
                    {
                        AppData.ActionEvents.OnNetworkConnectedEvent();
                        callbackResults.result = $"Network Successfully Connected For Device : [{AppData.Helpers.GetDeviceInfo().deviceName}] - Model : [{AppData.Helpers.GetDeviceInfo().deviceModel}] - ID : [{AppData.Helpers.GetDeviceInfo().deviceID}]";
                    }
                    else
                    {
                        surfacingManagerInstance.ShowPopUp(AppData.PopupTemplateType.NetworkNotificationPopUp, popUpShownCallbackResults =>
                        {
                            callbackResults.SetResult(popUpShownCallbackResults);

                            if (callbackResults.Success())
                            {
                                AppData.ActionEvents.OnNetworkFailedEvent();

                                callbackResults.result = $"Network Failed With Status : {status}";
                                callbackResults.resultCode = AppData.Helpers.WarningCode;
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                        }, OnNetworkRetryButtonOverrideEvent);
                    }
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }

            return callbackResults;
        }

        public async Task<AppData.Callback> OnCheckNetworkConnectionStatus()
        {
            AppData.Callback callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(SurfacingManager.Instance, "Surfacing Manager", "Check Connection Status Failed - Surfacing Manager Instance Is Not Yet Initialized - Invalid Operation."));

            if (callbackResults.Success())
            {
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

                if (callbackResults.Success())
                {
                    AppData.ActionEvents.OnNetworkConnectedEvent();
                    callbackResults.result = $"Network Successfully Connected For Device : [{AppData.Helpers.GetDeviceInfo().deviceName}] - Model : [{AppData.Helpers.GetDeviceInfo().deviceModel}] - ID : [{AppData.Helpers.GetDeviceInfo().deviceID}]";
                }
                else
                {
                    AppData.ActionEvents.OnNetworkFailedEvent();

                    callbackResults.result = $"Network Failed With Status : {status}";
                    callbackResults.resultCode = AppData.Helpers.WarningCode;
                }
            }

            return callbackResults;
        }

        private async void OnNetworkRetryButtonOverrideEvent()
                  => await CheckConnectionStatus();

        int NetworkConnectionDelay()
        {
            var sceneAssetsManagerCallbackResults = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, "Scene Assets Manager Instance Is Not Yet initialized.");

            if (sceneAssetsManagerCallbackResults.Success())
            {
                var sceneAssetsManager = sceneAssetsManagerCallbackResults.data;
                return AppData.Helpers.ConvertSecondsFromFloatToMillisecondsInt(sceneAssetsManager.GetDefaultExecutionValue(AppData.RuntimeExecution.NetworkInitializationDefaultDuration).value);
            }
            else
                Log(sceneAssetsManagerCallbackResults.GetResultCode, sceneAssetsManagerCallbackResults.GetResult, this);

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
                Log(sceneAssetsManagerCallbackResults.GetResultCode, sceneAssetsManagerCallbackResults.GetResult, this);

            return 0;
        }

        #endregion

        #endregion
    }
}