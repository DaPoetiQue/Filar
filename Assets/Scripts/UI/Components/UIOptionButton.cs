using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.RedicalGames.Filar
{
    [RequireComponent(typeof(Button))]
    public class UIOptionButton : AppMonoBaseClass
    {

        #region Components

        [Space(10)]
        [SerializeField]
        private TabPanelComponent panel = null;

        [Space(10)]
        [SerializeField]
        private InputActionHandler arrowIconHandler = null; 

        [Space(10)]
        [SerializeField]
        private List<UIOptionButton> options = new List<UIOptionButton>();

        private Button button;

        private bool isShowing = false;

        #endregion

        #region Main

        public void Initialize(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetButton());

            if(callbackResults.Success())
            {
                callbackResults.SetResult(GetPanel());

                if (callbackResults.Success())
                {
                    GetPanel().GetData().HidePanel(panelHiddenCallbackResults => 
                    {
                        callbackResults.SetResult(panelHiddenCallbackResults);

                        if (callbackResults.Success())
                        {
                            callbackResults.SetResult(GetArrowIconHandler());

                            if (callbackResults.Success())
                            {
                                var iconHandler = GetArrowIconHandler().GetData();

                                iconHandler.Init<AppData.ImageConfigDataPacket>(initializationCallbackResults => 
                                {
                                    callbackResults.SetResult(initializationCallbackResults);
                                
                                    if(callbackResults.Success())
                                    {
                                        callbackResults.SetResult(iconHandler.GetImageComponent());

                                        if (callbackResults.Success())
                                        {
                                            var actionImage = iconHandler.GetImageComponent().GetData();

                                            callbackResults.SetResult(actionImage.Initialized());

                                            if (callbackResults.Success())
                                            {
                                                actionImage.Initialize(initializationCallbackResults =>
                                                {
                                                    callbackResults.SetResult(initializationCallbackResults);

                                                    if(callbackResults.Success())
                                                    {
                                                        callbackResults.SetResult(GetOptions());

                                                        if (callbackResults.Success())
                                                        {
                                                            actionImage.SetUIInputState(AppData.InputUIState.Shown);

                                                            if (callbackResults.Success())
                                                            {
                                                                GetButton().GetData().onClick.AddListener(() =>
                                                                {
                                                                    OnClickEvent(clickCallbackResults =>
                                                                    {
                                                                        callbackResults.SetResult(clickCallbackResults);
                                                                    });
                                                                });
                                                            }
                                                            else
                                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                            if(callbackResults.Success())
                                                            {
                                                                var options = GetOptions().GetData();

                                                                for (int i = 0; i < options.Count; i++)
                                                                {
                                                                    callbackResults.SetResult(options[i].GetOptions());

                                                                    if (callbackResults.Success())
                                                                    {
                                                                        options[i].Initialize(callback: panelClosedCallbackResults =>
                                                                        {
                                                                            callbackResults.SetResult(panelClosedCallbackResults);
                                                                        });
                                                                    }
                                                                    else
                                                                    {
                                                                        options[i].Initialize(OnClosePanel, panelClosedCallbackResults => 
                                                                        {
                                                                            callbackResults.SetResult(panelClosedCallbackResults);
                                                                        });
                                                                    }
                                                                }
                                                            }
                                                            else
                                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                        }
                                                        else
                                                        {
                                                            callbackResults.result = $"{GetName()} Doesn't Contain Any Options.";
                                                            callbackResults.resultCode = AppData.Helpers.SuccessCode;

                                                            actionImage.SetUIInputState(AppData.InputUIState.Hidden);

                                                            GetButton().GetData().onClick.AddListener(() =>
                                                            {
                                                                OnClickEvent(clickCallbackResults =>
                                                                {
                                                                    callbackResults.SetResult(clickCallbackResults);
                                                                });
                                                            });
                                                        }
                                                    }
                                                    else
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                });
                                            }
                                            else
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        }
                                        else
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    });
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void Initialize(Action baseAction, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetButton());

            if (callbackResults.Success())
            {
                callbackResults.SetResult(GetPanel());

                if (callbackResults.Success())
                {
                    GetPanel().GetData().HidePanel(panelHiddenCallbackResults =>
                    {
                        callbackResults.SetResult(panelHiddenCallbackResults);

                        if (callbackResults.Success())
                        {
                            callbackResults.SetResult(GetArrowIconHandler());

                            if (callbackResults.Success())
                            {
                                var iconHandler = GetArrowIconHandler().GetData();

                                iconHandler.Init<AppData.ImageConfigDataPacket>(initializationCallbackResults =>
                                {
                                    callbackResults.SetResult(initializationCallbackResults);

                                    if (callbackResults.Success())
                                    {
                                        callbackResults.SetResult(iconHandler.GetImageComponent());

                                        if (callbackResults.Success())
                                        {
                                            var actionImage = iconHandler.GetImageComponent().GetData();

                                            callbackResults.SetResult(actionImage.Initialized());

                                            if (callbackResults.Success())
                                            {
                                                actionImage.Initialize(initializationCallbackResults =>
                                                {
                                                    callbackResults.SetResult(initializationCallbackResults);

                                                    if (callbackResults.Success())
                                                    {
                                                        callbackResults.SetResult(GetOptions());

                                                        if (callbackResults.Success())
                                                        {
                                                            actionImage.SetUIInputState(AppData.InputUIState.Shown);

                                                            if (callbackResults.Success())
                                                            {
                                                                GetButton().GetData().onClick.AddListener(() =>
                                                                {
                                                                    OnClickEvent(clickCallbackResults =>
                                                                    {
                                                                        callbackResults.SetResult(clickCallbackResults);
                                                                    });
                                                                });
                                                            }
                                                            else
                                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                                                            if (callbackResults.Success())
                                                            {
                                                                var options = GetOptions().GetData();

                                                                for (int i = 0; i < options.Count; i++)
                                                                {
                                                                    callbackResults.SetResult(options[i].GetOptions());

                                                                    if (callbackResults.Success())
                                                                    {
                                                                        options[i].Initialize(callback: panelClosedCallbackResults => 
                                                                        {
                                                                            callbackResults.SetResult(panelClosedCallbackResults);
                                                                        });
                                                                    }
                                                                    else
                                                                    {
                                                                        options[i].Initialize(OnClosePanel, panelClosedCallbackResults =>
                                                                        {
                                                                            callbackResults.SetResult(panelClosedCallbackResults);
                                                                        });
                                                                    }
                                                                }
                                                            }
                                                            else
                                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                        }
                                                        else
                                                        {
                                                            callbackResults.result = $"{GetName()} Doesn't Contain Any Options.";
                                                            callbackResults.resultCode = AppData.Helpers.SuccessCode;

                                                            actionImage.SetUIInputState(AppData.InputUIState.Hidden);

                                                            GetButton().GetData().onClick.AddListener(() =>
                                                            {
                                                                OnClickEvent(clickCallbackResults =>
                                                                {
                                                                    callbackResults.SetResult(clickCallbackResults);

                                                                    if (callbackResults.Success())
                                                                        baseAction.Invoke();
                                                                    else
                                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                                });
                                                            });
                                                        }
                                                    }
                                                    else
                                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                                });
                                            }
                                            else
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        }
                                        else
                                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                });
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    });
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        private void OnClosePanel()
        {

        }

        private void OnClickEvent(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetPanel());

            if (callbackResults.Success())
            {
                isShowing = !isShowing;

                if(isShowing)
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

        private AppData.CallbackData<Button> GetButton()
        {
            var callbackResults = new AppData.CallbackData<Button>();

            button = GetComponent<Button>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(button, "Button", $"Get Button Failed - Couldn't Get Button For : {GetName()} - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Button Success - Found Button For : {GetName()}.";
                callbackResults.data = button;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<TabPanelComponent> GetPanel()
        {
            var callbackResults = new AppData.CallbackData<TabPanelComponent>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(panel, "Panel", $"Get Panel Failed - Couldn't Get Panel For : {GetName()} - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Panel Success - Found Panel For : {GetName()}.";
                callbackResults.data = panel;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackDataList<UIOptionButton> GetOptions()
        {
            var callbackResults = new AppData.CallbackDataList<UIOptionButton>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(options, "Options", $"Get Options Failed - Couldn't Get Options For : {GetName()} - There Are No Options Assigned In The Inspector Panel - Invalid Operation."));

            if(callbackResults.Success())
            {
                callbackResults.result = $"Get Options Success - {options.Count} Options Have Been Found For : {GetName()}.";
                callbackResults.data = options;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<InputActionHandler> GetArrowIconHandler()
        {
            var callbackResults = new AppData.CallbackData<InputActionHandler>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(arrowIconHandler, "Arrow Icon Handler", $"Get Arrow Icon Handler For : {GetName()} Failed - There Is No Arrow Icon Handler Assigned - Invalid Operation."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Arrow Icon Handler For : {GetName()} Success - Arrow Icon Handler Is Assigned For : {GetName()}.";
                callbackResults.data = arrowIconHandler;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        #endregion
    }
}