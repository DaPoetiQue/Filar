using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.RedicalGames.Filar
{
    public class PostManager : AppData.SingletonBaseComponent<PostManager>
    {
        #region Components

        private AppData.Post post;
        private List<AppData.Post> posts;
        private List<ScenePostContentHandler> postsContents = new List<ScenePostContentHandler>();

        public bool HasPost { get; private set; }

        public int PostCount { get; private set; }

        #endregion

        #region Main

        protected override void Init()
        {
            
        }

        #region Data Setters

        public void SetPost(AppData.Post post, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(post, "Post", "Post Parameter Value Is Null / Invalid. Please Check Post Manager For A possible Fix."));

            if (callbackResults.Success())
            {
                this.post = post;
                HasPost = true;
                callbackResults.result = $"A Post Have Been Assigned Successfully.";
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void RemovePost(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetPost());

            if (callbackResults.Success())
            {
                this.post = null;
                HasPost = false;

                callbackResults.result = $"A Post Have Been Removed Successfully.";
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void RemovePost(AppData.Post post, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetPosts());

            if(callbackResults.Success())
            {
                if (this.post == post)
                {
                    this.post = null;
                    HasPost = false;
                }

                GetPosts().GetData().Remove(post);

                callbackResults.result = $"A Post Have Been Removed Successfully.";
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

        public void AddPostsContents(Action<AppData.Callback> callback = null, params ScenePostContentHandler[] contents)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentsValid(contents, "Contents", "Failed To Add Posts Contents - Content Params Are Invalid / Null."));

            if(callbackResults.Success())
            {
                foreach (var content in contents)
                {
                    if(!postsContents.Contains(content))
                    {
                        postsContents.Add(content);

                        callbackResults.result = $"Post Content : {content.GetName()} Have Been Successfully Added To Post Contents";
                        callbackResults.resultCode = AppData.Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.result = $"Post Content : {content.GetName()} Already Exists In Post Contents";
                        callbackResults.resultCode = AppData.Helpers.WarningCode;

                        break;
                    }
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        //public void LoadDefaultPostContent(Action<AppData.Callback> callback = null)
        //{
        //    var callbackResults = new AppData.Callback(GetPost());

        //    if (callbackResults.Success())
        //    {
        //        callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance Is Not Yet Initialized."));

        //        if (callbackResults.Success())
        //        {
        //            var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance Is Not Yet Initialized.").GetData();

        //            appDatabaseManagerInstance.LoadPostContent(post, postLoadedCallbackResults =>
        //            {
        //                callbackResults.SetResult(postLoadedCallbackResults);
        //            });
        //        }
        //        else
        //            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
        //    }
        //    else
        //        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

        //    callback?.Invoke(callbackResults);
        //}

        public void SelectPost(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetPostContent(post));

            if (callbackResults.Success())
            {
                SetPost(post, postSetCallbackResults =>
                {
                    callbackResults.SetResult(postSetCallbackResults);

                    if (callbackResults.Success())
                    {
                        var postContent = GetPostContent(post).GetData();
                        postContent.ShowContent();
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                });
            }

            callback?.Invoke(callbackResults);
        }

        public void SelectPost(AppData.Post post, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetPostContent(post));

            if (callbackResults.Success())
            {
                SetPost(post, postSetCallbackResults =>
                {
                    callbackResults.SetResult(postSetCallbackResults);

                    if (callbackResults.Success())
                    {
                        var postContent = GetPostContent(post).GetData();
                        postContent.ShowContent();
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                });
            }

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

        public AppData.CallbackData<ScenePostContentHandler> GetPostContent(AppData.Post post)
        {
            var callbackResults = new AppData.CallbackData<ScenePostContentHandler>(AppData.Helpers.GetAppComponentsValid(postsContents, "Posts Contents", "Failed To Get Posts Contents - Posts Contents Are Not Loaded - Please Check Post Manager For A Possible Fix."));

            if (callbackResults.Success())
            {
                var postContent = postsContents.Find(content => content.GetPost().GetData() == post);

                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(postContent, "Post Content", $"Failed To Find Post Content For Post Titled : {post.GetTitle()} - Of ID : {post.GetIdentifier()}"));

                if (callbackResults.Success())
                {
                    callbackResults.result = $"{postsContents.Count} Post(s) Content(s) Have Been Loaded Successfully.";
                    callbackResults.data = postContent;
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackDataList<ScenePostContentHandler> GetPostsContents()
        {
            var callbackResults = new AppData.CallbackDataList<ScenePostContentHandler>(AppData.Helpers.GetAppComponentsValid(postsContents, "Posts Contents", "Failed To Get Posts Contents - Posts Contents Are Not Loaded - Please Check Post Manager For A Possible Fix."));

            if (callbackResults.Success())
            {
                callbackResults.result = $"{postsContents.Count} Post(s) Content(s) Have Been Loaded Successfully.";
                callbackResults.data = postsContents;
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        #endregion

        #region Events

        #region Post Functions

        public void RefreshPosts(int refrenshDuration = 2000, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance", "Screen UI Manager Instance Is Not Yet Initialized."));

            if (callbackResults.Success())
            {
                var screenUIManagerInstance = AppData.Helpers.GetAppComponentValid(ScreenUIManager.Instance, "Screen UI Manager Instance").GetData();

                screenUIManagerInstance.GetCurrentScreen(currentScreenCallbackResults =>
                {
                    callbackResults.SetResult(currentScreenCallbackResults);

                    var screen = currentScreenCallbackResults.GetData();

                    screen.ShowWidget(AppData.WidgetType.LoadingWidget, async widgetShownCallbackResults =>
                    {
                        callbackResults.SetResult(widgetShownCallbackResults);

                        if (callbackResults.Success())
                        {
                            await Task.Delay(refrenshDuration);

                            SelectPost(postSelectedCallbacKResults =>
                            {
                                callbackResults.SetResult(postSelectedCallbacKResults);

                                if (callbackResults.Success())
                                    screen.HideScreenWidget(AppData.WidgetType.LoadingWidget);
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            });
                        }
                        else
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                    });
                });
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        #endregion

        #endregion

        #endregion
    }
}