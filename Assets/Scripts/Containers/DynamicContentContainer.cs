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

        #region Overrides

        protected override void OnInitialization()
        {

        }

        protected override void OnClear(bool showSpinner = false, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback(GetContainer<Transform>());

            if (callbackResults.Success())
            {
                var container = GetContainer<Transform>().data;

                if (ScreenUIManager.Instance.HasCurrentScreen().Success())
                {
                    if (GetContentCount().data > 0)
                    {
                        for (int i = 0; i < GetContentCount().data; i++)
                        {
                            var contentHandler = container.GetChild(i).GetComponent<PostContentHandler>();

                            if (contentHandler != null)
                            {
                                if (recycleContainer != null)
                                {
                                    recycleContainer.AddContent(contentHandler, false, contentAddedCallbackResults => 
                                    {
                                        callback.Invoke(contentAddedCallbackResults);
                                    });
                                }
                                else
                                {
                                    callbackResults.result = $"Recycle Container Is Not Yet Initialized.";
                                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                                }
                            }
                            else
                                LogError($"Widget : {container.GetChild(i).name} Doesn't Contain AppData.UIScreenWidget Component", this);
                        }
                    }
                    else
                    {
                        callbackResults.result = $"No Widgets To Clear From Container : {gameObject.name}";
                        callbackResults.resultCode = AppData.Helpers.SuccessCode;
                    }
                }
                else
                {
                    callbackResults.result = $"Curent Screen Is Not Yet Initialized.";
                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                }
            }

            callback?.Invoke(callbackResults);
        }

        protected override Task<AppData.Callback> OnClearAsync(bool showSpinner = false)
        {
            return null;
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