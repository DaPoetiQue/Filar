using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class UserHelpManager : AppData.SingletonBaseComponent<UserHelpManager>
    {
        #region Components

        [SerializeField]
        List<AppData.TutorialInfoView> tutorialInfoViews = new List<AppData.TutorialInfoView>();

        #endregion

        #region Unity Callbacks

        #endregion

        #region Main

        protected override void Init()
        {

        }

        public void GetTutorialView(AppData.WidgetType widgetType, Action<AppData.CallbackData<AppData.TutorialInfoView>> callback)
        {
            AppData.CallbackData<AppData.TutorialInfoView> callbackResults = new AppData.CallbackData<AppData.TutorialInfoView>();

            var tutorialView = tutorialInfoViews.Find(view => view.GetDataPackets().widgetType == widgetType);

            if(tutorialView != null)
            {
                callbackResults.result = $"Tutorial View Of Type : {widgetType} Found In Tutorial Views.";
                callbackResults.data = tutorialView;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = $"Tutorial View Of Type : {widgetType} Not Found In Tutorial Views.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback.Invoke(callbackResults);
        }

        public void GetTutorialView(AppData.TutorialInfoType infoType, Action<AppData.CallbackData<AppData.TutorialInfoView>> callback)
        {

        }

        #endregion
    }
}