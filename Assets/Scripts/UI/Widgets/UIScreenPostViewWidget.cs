using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class UIScreenPostViewWidget : AppData.UIScreenWidget
    {
        #region Components

        #endregion

        #region Unity Callbacks

        void Start() => Initialization();

        #endregion

        #region Main

        void Initialization()
        {
            // Initialize Assets.
            Init((callback) =>
            {
                if (AppData.Helpers.IsSuccessCode(callback.resultCode))
                    if (screenManager == null)
                        screenManager = ScreenUIManager.Instance;
                    else
                        Debug.LogWarning($"--> Failed to Initialize Scene Asset UI With Results : {callback.result}.");
                else
                    Debug.LogWarning("--> Failed to Initialize Scene Asset UI.");
            });
        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonDataPackets> actionButton)
        {
            switch (actionButton.dataPackets.action)
            {
                case AppData.InputActionButtonType.SelectPostButton:

                    AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, appDatabaseManagerCallbackResults =>
                    {
                        if (appDatabaseManagerCallbackResults.Success())
                        {
                            var appDatabaseManager = appDatabaseManagerCallbackResults.data;
                            appDatabaseManager.LoadSelectedPostContent(post.GetUniqueIdentifier());
                        }
                        else
                            Log(appDatabaseManagerCallbackResults.ResultCode, appDatabaseManagerCallbackResults.Result, this);

                    }, "App Database Manager Is Not Yet Initialized.");

                    break;
            }
        }

        void OnGoToProfile_ActionEvent(AppData.ButtonDataPackets dataPackets)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSetUIWidgetData(AppData.Folder folder)
        {
            throw new System.NotImplementedException();
        }

        public override void OnSelect(bool isInitialSelection = false)
        {
            if (SelectableManager.Instance != null)
            {
                Debug.LogError("===========> Please Fix Selection Here");
                //SelectableManager.Instance.Select(this, dataPackets, isInitialSelection);
                //Selected();
            }
            else
                Debug.LogWarning("--> OnSelect Failed :  SelectableManager.Instance Is Not Yet initialized.");
        }

        public override void OnDeselect() => Deselected();

        protected override void OnSetAssetData(AppData.SceneAsset assetData)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnScreenUIRefreshed()
        {

        }

        protected override void OnSetUIWidgetData(AppData.ProjectStructureData structureData)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnSetUIWidgetData(AppData.Post post)
        {
            #region Post 

            SetUITextDisplayerValue(post.GetTitle(), AppData.ScreenTextType.TitleDisplayer);

            #endregion

            #region Post Caption

            SetUITextDisplayerValue(post.GetCaption(), AppData.ScreenTextType.MessageDisplayer);

            #endregion

            #region Post Date Time

            LogInfo($" =>>>>>> Date Time : {new DateTime(post.creationDateTime)}");

            var postCreationDateTime = AppData.Helpers.GetElapsedTime(new AppData.DateTimeComponent(new DateTime(post.creationDateTime)));
            SetUITextDisplayerValue(postCreationDateTime, AppData.ScreenTextType.DateTimeDisplayer);

            #endregion

            #region Thumbnail


            //if (post.ThumbnailAssigned())
            //{
            //    GetUIImageDisplayer(AppData.ScreenImageType.Thumbnail, thumbnailCallbackResults =>
            //    {
            //        if (thumbnailCallbackResults.Success())
            //            SetUIImageDisplayerValue(post.GetTexture2DThumbnail(thumbnailCallbackResults.data.mainTexture.width, thumbnailCallbackResults.data.mainTexture.width), AppData.ScreenImageType.Thumbnail);
            //        else
            //            Log(thumbnailCallbackResults.ResultCode, thumbnailCallbackResults.Result, this);
            //    });
            //}
            //else
            //{
            //    AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, appDatabaseManagerCallbackResults => 
            //    {
            //        if(appDatabaseManagerCallbackResults.Success())
            //        {
            //            var appDatabaseManager = appDatabaseManagerCallbackResults.data;
            //            SetUIImageDisplayerValue(appDatabaseManager.GetImageFromLibrary(AppData.UIImageType.ImagePlaceholder).value, AppData.ScreenImageType.Thumbnail);
            //        }
            //        else
            //            Log(appDatabaseManagerCallbackResults.ResultCode, appDatabaseManagerCallbackResults.Result, this);

            //    },  "App Database Manager Is Not Yet Initialized.");
            //}

            #endregion
        }

        #endregion
    }
}