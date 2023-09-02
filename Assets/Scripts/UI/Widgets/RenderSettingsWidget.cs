using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class RenderSettingsWidget : AppData.Widget
    {
        #region Components

        [SerializeField]
        AppData.NavigationWidget navigationWidget = new AppData.NavigationWidget();

        bool widgetsInitialized = false;

        #endregion

        #region Unity Callbacks

        void OnEnable() => RegisterEventListeners(true);
        void OnDisable() => RegisterEventListeners(false);

        #endregion

        #region Main

        void RegisterEventListeners(bool register)
        {
            if (register)
            {
                AppData.ActionEvents._OnNavigationSubTabChangedEvent += ActionEvents__OnNavigationSubTabChangedEvent;
                AppData.ActionEvents._OnNavigationTabWidgetEvent += ActionEvents__OnNavigationTabWidgetEvent;
            }
            else
            {
                AppData.ActionEvents._OnNavigationSubTabChangedEvent -= ActionEvents__OnNavigationSubTabChangedEvent;
                AppData.ActionEvents._OnNavigationTabWidgetEvent -= ActionEvents__OnNavigationTabWidgetEvent;
            }
        }

        private void ActionEvents__OnNavigationSubTabChangedEvent(AppData.NavigationTabID tabID, AppData.NavigationRenderSettingsProfileID navigationSelection)
        {
            Debug.LogError($"--> Navigation Canged For Tab : {tabID} With Selection : {navigationSelection}");

            if (navigationWidget != null)
            {
                AppData.NavigationTab subWidget = navigationWidget.navigationTabsList.Find((x) => x.navigationID == tabID);

                if (subWidget != null)
                {
                    if (subWidget.navigationSubWidget != null)
                        subWidget.navigationSubWidget.OnNavigationTabChangedEvent(navigationSelection);
                }
                else
                    Debug.LogWarning($"--> RG_Unity - ActionEvents__OnCreateNewRenderProfileEvent Failed : Sub Widget Not Found For : {tabID}");
            }
        }

        private void ActionEvents__OnNavigationTabWidgetEvent(AppData.ButtonDataPackets dataPackets)
        {
            if (navigationWidget != null)
            {
                AppData.NavigationTab subWidget = navigationWidget.navigationTabsList.Find((x) => x.navigationID == dataPackets.tabID);

                if (subWidget != null)
                {
                    switch (dataPackets.navigationWidgetVisibilityState)
                    {
                        case AppData.NavigationWidgetVisibilityState.Show:

                            subWidget.ShowWidget(dataPackets.navigationTabWidgetType, (callback) =>
                            {
                                if (!AppData.Helpers.IsSuccessCode(callback.resultCode))
                                    Debug.LogError($"------> Failed To Show Tab Widget : {dataPackets.navigationTabWidgetType} With Error Result : {callback.result}");

                            });

                            break;

                        case AppData.NavigationWidgetVisibilityState.Hide:

                            subWidget.HideWidget(dataPackets.navigationTabWidgetType, (callback) =>
                            {
                                if (!AppData.Helpers.IsSuccessCode(callback.resultCode))
                                    Debug.LogError($"------> Failed : {dataPackets.navigationTabWidgetType} With Error Result : {callback.result}");

                            });

                            break;
                    }
                }
                else
                    Debug.LogWarning($"--> RG_Unity - ActionEvents__OnCreateNewRenderProfileEvent Failed : Sub Widget Not Found For : {dataPackets.navigationTabWidgetType}");
            }
        }

        void InititializeWidget()
        {
            renderSettingsWidget = this;

            if (navigationWidget != null)
            {
                navigationWidget.Init((status) =>
                {
                    if (!AppData.Helpers.IsSuccessCode(status.resultCode))
                        Debug.LogWarning($"--> Init Failed With Results : {status.result}");
                });
            }
            else
                Debug.LogWarning("--> RG_Unity - Init Failed : Navigation Widget Is Null.");

            Init();
        }


        protected override void OnScreenWidget()
        {

        }

        void SetWidgetAssetData(AppData.SceneAsset asset)
        {
            //AppData.Helpers.ShowImage(asset, thumbnailDisplayer);

            if (titleDisplayer != null && !string.IsNullOrEmpty(asset.name))
                titleDisplayer.text = asset.name;
        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);

            if (!widgetsInitialized)
            {
                InititializeWidget();
                widgetsInitialized = true;
            }

            if (AppDatabaseManager.Instance)
                SetWidgetAssetData(AppDatabaseManager.Instance.GetCurrentSceneAsset());
            else
                Debug.LogWarning("--> Scene Assets Manager Not Yet Initialized.");
        }

        protected override void OnHideScreenWidget()
        {
            HideSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void ScrollerPosition(Vector2 position)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }

}