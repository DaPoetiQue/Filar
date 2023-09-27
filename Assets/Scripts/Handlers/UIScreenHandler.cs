using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.RedicalGames.Filar
{
    public class UIScreenHandler : AppData.ScreenUIData
    {
        #region Unity Callbacks

        void OnEnable() => ActionEventsSubscription(true);

        void OnDisable() => ActionEventsSubscription(false);

        #endregion

        #region Main

        public async void Init()
        {
            if (screenWidgetsList == null || screenWidgetsList.Count == 0)
            {
                AppData.Widget[] popUpComponents = this.GetComponentsInChildren<AppData.Widget>();

                if (popUpComponents.Length > 0)
                {
                    screenWidgetsList = new List<AppData.Widget>();

                    foreach (var widget in popUpComponents)
                    {
                        if (widget != null && !screenWidgetsList.Contains(widget))
                        {
                            await Task.Yield();

                            widget.Init(this, initializationCallbackResults => 
                            {
                                if (initializationCallbackResults.Success())
                                {
                                    var widgetEventActionData = initializationCallbackResults.GetData();

                                    RegisterEventAction(eventRegisteredCallbackResults => 
                                    {
                                        if (eventRegisteredCallbackResults.Success())
                                            screenWidgetsList.Add(widget);
                                        else
                                            Log(eventRegisteredCallbackResults.GetResultCode, eventRegisteredCallbackResults.GetResult, this);

                                    }, widgetEventActionData);
                                }
                                else
                                    Log(initializationCallbackResults.GetResultCode, initializationCallbackResults.GetResult, this);
                            });
                        }
                        else
                            break;
                    }
                }
            }
        }

        void ActionEventsSubscription(bool subscribe)
        {
            if (subscribe)
            {
                AppData.ActionEvents._OnScreenChangedEvent += OnScreenChangedEvent;
                AppData.ActionEvents._OnPopUpActionEvent += OnWidgetsEvents;
                AppData.ActionEvents._OnScreenTogglableStateEvent += OnScreenTogglableStateEvent;
                AppData.ActionEvents._OnSceneModelPoseResetEvent += OnAssetPoseReset;
            }
            else
            {
                AppData.ActionEvents._OnPopUpActionEvent -= OnWidgetsEvents;
                AppData.ActionEvents._OnScreenChangedEvent -= OnScreenChangedEvent;
                AppData.ActionEvents._OnScreenTogglableStateEvent -= OnScreenTogglableStateEvent;
                AppData.ActionEvents._OnSceneModelPoseResetEvent -= OnAssetPoseReset;
            }
        }

        #endregion
    }
}
