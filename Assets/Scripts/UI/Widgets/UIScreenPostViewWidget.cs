using System;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class UIScreenPostViewWidget : AppData.SelectableWidget
    {
        #region Main

        protected override void OnInitilize(Action<AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType, AppData.Widget>>> callback)
        {
            var callbackResults = new AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType, AppData.Widget>>();

            // Initialize Assets.
            Init(initializationCallbackResults =>
            {
                callbackResults.SetResult(initializationCallbackResults);
            });

            callback.Invoke(callbackResults);
        }

        #region Post Data

        private void SetPost(AppData.Post post, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(post, "Post", $"Set Post Failed - Post Parameter Value For : {GetName()} - Of Type : {GetType().GetData()} Is Not Assigned - Invalid Operation"));

            if(callbackResults.Success())
            {
                this.post = post;
                callbackResults.result = $"Set Post Success - Post Value For : {GetName()} - Of Type : {GetType().GetData()} Is Assigned Post : {post.GetName()}.";
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public AppData.CallbackData<AppData.Post> GetPost()
        {
            var callbackResults = new AppData.CallbackData<AppData.Post>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(post, "Post", $"Get Post Failed - Post Value For : {GetName()} - Of Type : {GetType().GetData()} Is Not Assigned - Invalid Operation"));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Get Post Success - Post Value For : {GetName()} - Of Type : {GetType().GetData()} Is Assigned.";
                callbackResults.data = post;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        #endregion

        protected override AppData.CallbackData<AppData.WidgetStatePacket<AppData.SelectableWidgetType, AppData.WidgetType, AppData.Widget>> OnGetState()
        {
            return null;
        }

        protected override void OnActionButtonInputs(AppData.UIButton<AppData.ButtonConfigDataPacket> actionButton)
        {
           
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

            SetPost(post, postSetCallbackResults => 
            {
                callbackResults.SetResult(postSetCallbackResults);

                if (callbackResults.Success())
                {
                    #region Thumbnail

                    callbackResults.SetResult(GetUIImageDisplayer(AppData.ScreenImageType.Thumbnail));

                    if (callbackResults.Success())
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
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            });
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

            var callbackResults = new AppData.Callback();

            callbackResults.SetResult(AppData.Helpers.GetAppEnumValueValid(actionType, "Action Type", $"On Action Button Event Failed - Action Type Parameter Value Is Set To Default : {actionType} - Invalid Operation"));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Screen UI Manager Instance Is Not Yet Initialized."));

                if (callbackResults.Success())
                {
                    var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                    callbackResults.SetResult(screenUIManagerInstance.GetCurrentScreen());

                    if (callbackResults.Success())
                    {
                        var screen = screenUIManagerInstance.GetCurrentScreen().GetData();

                        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(PostManager.Instance, "Post Manager Instance", "Post Manager Instance Is Not Yet Initialized."));

                        if (callbackResults.Success())
                        {
                            var postManagerInstance = AppData.Helpers.GetAppComponentValid(PostManager.Instance, "Post Manager Instance").GetData();

                            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance", "Profile Manager Instance Is Not Yet Initialized."));

                            if (callbackResults.Success())
                            {
                                var profileManagerInstance = AppData.Helpers.GetAppComponentValid(ProfileManager.Instance, "Profile Manager Instance").GetData();

                                switch (screenWidgetType)
                                {
                                    case AppData.SelectableWidgetType.Post:

                                        if (actionType == AppData.InputActionButtonType.SelectPostButton)
                                        {
                                            postManagerInstance.SelectPost(post, postSelectedCallbackResults =>
                                            {
                                                callbackResults.SetResult(postSelectedCallbackResults);
                                            });
                                        }
                                        else
                                        {
                                            callbackResults.SetResult(profileManagerInstance.OnCheckProfileSignIn());

                                            if (callbackResults.Success())
                                            {
                                                switch (actionType)
                                                {

                                                    case AppData.InputActionButtonType.LikePostButton:

                                                        break;

                                                    case AppData.InputActionButtonType.DislikePostButton:

                                                        break;

                                                    case AppData.InputActionButtonType.ViewCommentsButton:

                                                        break;

                                                    case AppData.InputActionButtonType.SharePostButton:

                                                        break;

                                                    case AppData.InputActionButtonType.AddToCartButton:

                                                        break;
                                                }
                                            }
                                            else
                                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                        }

                                        break;
                                }
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
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