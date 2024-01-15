using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class UIScreenPostViewWidget : AppData.SelectableWidget
    {
        #region Components

        #endregion

        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType>>();

            // Initialize Assets.
            Init(initializationCallbackResults =>
            {
                callbackResults.SetResult(initializationCallbackResults);
            });

            callback.Invoke(callbackResults);
        }


        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType>> OnGetState()
        {
            return null;
        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {
            //switch (actionButton.dataPackets.action)
            //{
            //    case AppData.InputActionButtonType.SelectPostButton:

            //        AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, async appDatabaseManagerCallbackResults =>
            //        {
            //            if (appDatabaseManagerCallbackResults.Success())
            //            {
            //                var appDatabaseManager = appDatabaseManagerCallbackResults.data;

            //                LogInfo(" +++++++++ Pressed Select", this);

            //                await appDatabaseManager.LoadSelectedPostContent(post);

            //                CancelActionButtonInvoke(AppData.InputActionButtonType.ConfirmationButton);
            //            }
            //            else
            //                Log(appDatabaseManagerCallbackResults.GetResultCode, appDatabaseManagerCallbackResults.GetResult, this);

            //        }, "App Database Manager Is Not Yet Initialized.");

            //        break;
            //}
        }

        void OnGoToProfile_ActionEvent(AppData.ButtonConfigDataPacket dataPackets)
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
            var callbackResults = new AppData.Callback();

            #region Thumbnail

            callbackResults.SetResult(GetUIImageDisplayer(AppData.ScreenImageType.Thumbnail));

            if(callbackResults.Success())
                SetUIImageDisplayerValue(post.GetPostThumbnail(), AppData.ScreenImageType.Thumbnail);

            #endregion

            #region Post Title

            SetUITextDisplayerValue(AppData.ScreenTextType.TitleDisplayer, post.GetTitle(), postTitleSetCallbackResults => 
            {
                callbackResults.SetResult(postTitleSetCallbackResults);       
            });

            #endregion

            #region Post Caption

            SetUITextDisplayerValue(AppData.ScreenTextType.MessageDisplayer, post.GetCaption(), postCaptionSetCallbackResults => 
            {
                callbackResults.SetResult(postCaptionSetCallbackResults);
            });

            #endregion

            #region Post Date Time

            var postCreationDateTime = AppData.Helpers.GetElapsedTime(new AppData.DateTimeComponent(new DateTime(post.creationDateTime)));

            SetUITextDisplayerValue(AppData.ScreenTextType.DateTimeDisplayer, postCreationDateTime, postDateTimeSetCallbackResults => 
            {
                callbackResults.SetResult(postDateTimeSetCallbackResults);
            });

            #endregion

            #region Post Likes Count Displayer

            SetUITextDisplayerValue(AppData.ScreenTextType.PostLikeCountDisplayer, "24", postLikesCountSetCallbackResults =>
            {
                callbackResults.SetResult(postLikesCountSetCallbackResults);
            });

            #endregion

            #region Post Dislikes Count Displayer

            SetUITextDisplayerValue(AppData.ScreenTextType.PostDislikeCountDisplayer, "2", postDislikesCountSetCallbackResults =>
            {
                callbackResults.SetResult(postDislikesCountSetCallbackResults);
            });

            #endregion

            #region Post Comments Count Displayer

            SetUITextDisplayerValue(AppData.ScreenTextType.PostCommentsCountDisplayer, "8", postCommentsCountSetCallbackResults =>
            {
                callbackResults.SetResult(postCommentsCountSetCallbackResults);
            });

            #endregion

            //#region Thumbnail

            ////AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, AppDatabaseManager.Instance.name, appDatabaseManagerCallbackResults =>
            ////{
            ////    if (appDatabaseManagerCallbackResults.Success())
            ////    {
            ////        var appDatabaseManager = appDatabaseManagerCallbackResults.data;

            ////        LogSuccess($" +++=================>>>>>>>>>> Get Post Results : {appDatabaseManager.GetPostContent(post).Result}", this);

            ////        //if (appDatabaseManager.GetPostContent(post).Success())
            ////        //{
            ////        //    LogSuccess($" +++=================>>>>>>>>>> Thumbnail Results : {appDatabaseManager.GetPostContent(post).Result}", this);
            ////        //}
            ////        //else
            ////        //    Log(appDatabaseManager.GetPostContent(post).ResultCode, appDatabaseManager.GetPostContent(post).Result, this);
            ////    }
            ////    else
            ////        Log(appDatabaseManagerCallbackResults.ResultCode, appDatabaseManagerCallbackResults.Result, this);
            ////});


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

            //#endregion
        }

        protected override void OnScreenWidgetShownEvent()
        {
           
        }

        protected override void OnScreenWidgetHiddenEvent()
        {
        
        }

        protected override void OnScreenWidgetTransitionInProgressEvent()
        {
          
        }

        protected override void OnActionButtonEvent(AppData.SelectableWidgetType screenWidgetType, AppData.InputActionButtonType actionType, AppData.SceneConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnActionDropdownValueChanged(int value, AppData.DropdownConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnScrollerValueChanged(Vector2 value)
        {
            throw new NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(string value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnInputFieldValueChanged(int value, AppData.InputFieldConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        protected override void OnCheckboxValueChanged(AppData.CheckboxInputActionType actionType, bool value, AppData.CheckboxConfigDataPacket dataPackets)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}