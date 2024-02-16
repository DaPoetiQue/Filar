using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class DynamicContentContainer : AppData.DynamicContainer
    {
        #region Components

        #endregion

        #region Main

        public (Vector3 position, Vector3 scale, Quaternion rotation) GetPose() => (transform.position, transform.localScale, transform.rotation);
        public (Vector3 position, Vector3 scale, Quaternion rotation) GetLocalPose() => (transform.localPosition, transform.localScale, transform.localRotation);

        #region Overrides

        protected override void OnInitialization()
        {

        }

        protected override void OnClear(bool showSpinner = false, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback(GetContainer<Transform>());

            if (callbackResults.Success())
            {
                var container = GetContainer<Transform>().GetData();

                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Screen UI Manager Instance Is Not Initialized Yet."));

                if (callbackResults.Success())
                {
                    var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                    callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                    if (callbackResults.Success())
                    {
                        var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                        callbackResults.SetResult(HasContent());

                        if (callbackResults.Success())
                        {
                            if (showSpinner)
                                screen.ShowWidget(AppData.WidgetType.LoadingWidget, true);

                            for (int i = 0; i < GetContentCount().GetData(); i++)
                            {
                                var contentHandler = container.GetChild(i).GetComponent<ScenePostContentHandler>();

                                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(contentHandler, "Content Handler", "Clear Container Failed - Content Handler Not Found."));

                                if (callbackResults.Success())
                                {
                                    callbackResults.SetResult(GetRecycleContainer());

                                    if (callbackResults.Success())
                                    {
                                        var recycleContainerObject = GetRecycleContainer().GetData();

                                        recycleContainerObject.AddContent(contentHandler, false, contentAddedCallbackResults =>
                                        {
                                            callbackResults.SetResult(contentAddedCallbackResults);
                                        });
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            }

                            if (callbackResults.Success())
                            {
                                if (showSpinner)
                                    screen.HideScreenWidget(AppData.WidgetType.LoadingWidget);
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        }
                        else
                        {
                            callbackResults.result = $"There Are No Contents To Clear For : {GetName()} - Of Type : {GetContainerType().GetData()}";
                            callbackResults.resultCode = AppData.Helpers.SuccessCode;
                        }
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }

            callback?.Invoke(callbackResults);
        }

        protected override async Task<AppData.Callback> OnClearAsync(bool showSpinner = false, float refreshDuration = 0.0f)
        {
            AppData.Callback callbackResults = new AppData.Callback(GetContainer<Transform>());

            if (callbackResults.Success())
            {
                var container = GetContainer<Transform>().GetData();

                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Screen UI Manager Instance Is Not Initialized Yet."));

                if (callbackResults.Success())
                {
                    var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                    callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                    if (callbackResults.Success())
                    {
                        var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                        callbackResults.SetResult(HasContent());

                        if (callbackResults.Success())
                        {
                            for (int i = 0; i < GetContentCount().GetData(); i++)
                            {
                                var contentHandler = container.GetChild(i).GetComponent<ScenePostContentHandler>();

                                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(contentHandler, "Content Handler", "Clear Container Failed - Content Handler Not Found."));

                                if (callbackResults.Success())
                                {
                                    callbackResults.SetResult(GetRecycleContainer());

                                    if (callbackResults.Success())
                                    {
                                        var recycleContainerObject = GetRecycleContainer().GetData();

                                        recycleContainerObject.AddContent(contentHandler, false, contentAddedCallbackResults =>
                                        {
                                            callbackResults.SetResult(contentAddedCallbackResults);
                                        });

                                        if (callbackResults.Success())
                                        {
                                            if (showSpinner)
                                            {
                                                screen.ShowWidget(AppData.WidgetType.LoadingWidget, widgetShownCallbackResults =>
                                               {
                                                   callbackResults.SetResult(widgetShownCallbackResults);
                                               });
                                            }

                                            await Task.Delay(AppData.Helpers.ConvertSecondsFromFloatToMillisecondsInt(refreshDuration));
                                        }
                                        else
                                        {
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                            break;
                                        }
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                }
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            }

                        }
                        else
                        {
                            callbackResults.result = $"There Are No Contents To Clear For : {GetName()} - Of Type : {GetContainerType().GetData()}";
                            callbackResults.resultCode = AppData.Helpers.SuccessCode;
                        }
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }

            return callbackResults;
        }

        protected override void OnContainerUpdate()
        {
            
        }

        protected override Task<AppData.Callback> OnContainerUpdateAsync()
        {
            return null;
        }

        protected override void OnUpdatedContainerSize(Action<AppData.CallbackData<Vector2>> callback = null)
        {
           
        }

        protected override Task<AppData.CallbackData<Vector2>> OnUpdatedContainerSizeAsync()
        {
            return null;
        }

        protected override void UpdateContentOrderInLayer(Action<AppData.Callback> callback = null)
        {
            LogInfo("This Type Of Container Doesn't Implement This Method.");
        }

        #endregion

        #endregion
    }
}