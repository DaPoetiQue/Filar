using System.Collections.Generic;

namespace Com.RedicalGames.Filar
{
    public class UIScreenHandler : AppData.ScreenUIData
    {
        #region Unity Callbacks

        void Start() => Init();

        void OnEnable() => ActionEventsSubscription(true);

        void OnDisable() => ActionEventsSubscription(false);

        #endregion

        #region Main

        void Init()
        {
            if (screenWidgetsList == null || screenWidgetsList.Count == 0)
            {
                AppData.Widget[] popUpComponents = this.GetComponentsInChildren<AppData.Widget>();

                if (popUpComponents.Length > 0)
                {
                    screenWidgetsList = new List<AppData.Widget>();

                    foreach (var popUpComponent in popUpComponents)
                    {
                        screenWidgetsList.Add(popUpComponent);
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
