using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class UserHelpInfoScreenWidget : AppData.Widget
    {
        #region Components

        [SerializeField]
        private AppData.TutorialInfoView view = new AppData.TutorialInfoView();

        #endregion

        #region Unity Callbacks
        void Start() => Init();

        #endregion

        #region Main

        new void Init()
        {
            userHelpInfoScreenWidget = this;
            base.Init();
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

        protected override void OnScreenWidget()
        {

        }

        protected override void OnShowScreenWidget(AppData.SceneDataPackets dataPackets)
        {
            ShowSelectedLayout(AppData.WidgetLayoutViewType.DefaultView);
        }

        protected override void OnSubscribeToActionEvents(bool subscribe)
        {
            LogInfo($"===============> Subscribe : {subscribe}", this);
        }

        protected override void OnScrollerValueChanged(Vector2 value) => scroller.Update();

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        public void SetTutorialView(AppData.TutorialInfoView view) => this.view = view;

        public void GetTutorialView(Action<AppData.CallbackData<AppData.TutorialInfoView>> callback)
        {
            AppData.CallbackData<AppData.TutorialInfoView> callbackResults = new AppData.CallbackData<AppData.TutorialInfoView>();

            if(view != null)
            {
                callbackResults.results = "View Found.";
                callbackResults.data = view;
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "View Is Not Assigned / Null.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        protected override void OnActionButtonEvent(AppData.WidgetType popUpType, AppData.InputActionButtonType actionType, AppData.SceneDataPackets dataPackets)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}