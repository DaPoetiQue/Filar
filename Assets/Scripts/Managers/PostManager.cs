using System;
using System.Collections.Generic;

namespace Com.RedicalGames.Filar
{
    public class PostManager : AppData.SingletonBaseComponent<PostManager>
    {
        #region Components

        private AppData.Post post;
        private List<AppData.Post> posts;

        public int PostCount { get; private set; }

        #endregion

        #region Main

        #region Data Setters

        public void SetPost(AppData.Post post, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(post, "Post", "Post Parameter Value Is Null / Invalid. Please Check Post Manager For A possible Fix."));

            if (callbackResults.Success())
            {
                this.post = post;
                callbackResults.result = $"A Post Have Been Assigned Successfully.";
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void SetPosts(List<AppData.Post> posts, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(posts, "Posts", "Posts Parameter Value Is Null / Invalid. Please Check Post Manager For A possible Fix."));

            if (callbackResults.Success())
            {
                this.posts = posts;
                PostCount = posts.Count;

                callbackResults.result = $"{PostCount} Posts Have Been Assigned Successfully.";
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void SelectPost(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetPost());

            if (callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance Is Not Yet Initialized."));

                if (callbackResults.Success())
                {
                    var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance Is Not Yet Initialized.").GetData();

                    appDatabaseManagerInstance.LoadSelectedPostContent(post, postLoadedCallbackResults =>
                    {
                        callbackResults.SetResult(postLoadedCallbackResults);
                    });
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void SelectPost(AppData.Post post, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

            SetPost(post, postSetCallbackResults => 
            {
                callbackResults.SetResult(postSetCallbackResults);

                if (callbackResults.Success())
                {
                    callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance Is Not Yet Initialized."));

                    if (callbackResults.Success())
                    {
                        var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance Is Not Yet Initialized.").GetData();

                        appDatabaseManagerInstance.LoadSelectedPostContent(post, postLoadedCallbackResults=> 
                        {
                            callbackResults.SetResult(postLoadedCallbackResults);
                        });
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            });

            callback?.Invoke(callbackResults);
        }

        #endregion

        #region Data Getters

        public AppData.CallbackData<AppData.Post> GetPost()
        {
            var callbackResults = new AppData.CallbackData<AppData.Post>(AppData.Helpers.GetAppComponentValid(post, "Post", "Failed To Get Post - Post Is Not Initialized - Please Check Post Manager For A Possible Fix."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"Post ID : {post.GetIdentifier()} - Have Been Loaded Successfully.";
                callbackResults.data = post;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackData<AppData.Post> GetPost(AppData.Profile profile)
        {
            var callbackResults = new AppData.CallbackData<AppData.Post>(GetPosts());

            if(callbackResults.Success())
            {
                // ToDo : Link profile To Posts. 
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackDataList<AppData.Post> GetPosts()
        {
            var callbackResults = new AppData.CallbackDataList<AppData.Post>(AppData.Helpers.GetAppComponentsValid(posts, "Posts", "Failed To Get Posts - Posts Are Not Initialized - Please Check Post Manager For A Possible Fix."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"{posts.Count} Post(s) Have Been Loaded Successfully.";
                callbackResults.data = posts;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        #endregion

        #endregion
    }
}