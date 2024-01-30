using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.RedicalGames.Filar
{
    [RequireComponent(typeof(Button))]
    public class UIOptionsToggle : AppData.UIBaseComponent
    {

        #region Components

        [Space(10)]
        [SerializeField]
        private List<AppData.UIScreenActionGroup> options = new List<AppData.UIScreenActionGroup>();

        private bool isShowing = false;

        #endregion

        #region Main

        protected override void OnInitialization(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetOptions());

            if(callbackResults.Success())
            {

            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        protected override void OnClickEvent(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

            callbackResults.SetResult(GetPanel());

            if (callbackResults.Success())
            {
                isShowing = !isShowing;

                if (isShowing)
                {
                    GetPanel().GetData().ShowPanel(panelShownCallbackResults => 
                    {
                        callbackResults.SetResult(panelShownCallbackResults);
                    });
                }
                else
                {
                    GetPanel().GetData().HidePanel(panelHiddenCallbackResults => 
                    {
                        callbackResults.SetResult(panelHiddenCallbackResults);
                    });
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        private AppData.CallbackDataList<AppData.UIScreenActionGroup> GetOptions()
        {
            var callbackResults = new AppData.CallbackDataList<AppData.UIScreenActionGroup>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(options, "Options", $"Get Options Failed - Couldn't Get Options For : {GetName()} - There Are No Options Assigned In The Inspector Panel - Invalid Operation."));

            if(callbackResults.Success())
            {

            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        #endregion
    }
}