using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class PostManager : AppData.SingletonBaseComponent<PostManager>
    {
        #region Components

        private AppData.Post post;
        private List<AppData.Post> posts;
        private List<ScenePostContentHandler> postsContents = new List<ScenePostContentHandler>();

        [SerializeField]
        private List<AppData.SelectableWidget> postWidgets = new List<AppData.SelectableWidget>();

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

        public void SelectPost(Action<AppData.CallbackData<AppData.Post>> callback = null)
        {
            var callbackResults = new AppData.CallbackData<AppData.Post>(GetPostContent(post));

            if (callbackResults.Success())
            {
                SetPost(post, postSetCallbackResults =>
                {
                    callbackResults.SetResult(postSetCallbackResults);

                    if (callbackResults.Success())
                    {
                        var postContent = GetPostContent(post).GetData();
                        postContent.ShowContent();

                        callbackResults.data = post;
                    }
                    else
                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

                });
            }

            callback?.Invoke(callbackResults);
        }

        public async void SelectPost(AppData.Post post, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetPostContent(post));

            if (callbackResults.Success())
            {
                callbackResults.SetResult(GetPost());

                if (callbackResults.Success())
                {
                    if (post != GetPost().GetData())
                    {
                        SetPost(post, async postSetCallbackResults =>
                        {
                            callbackResults.SetResult(postSetCallbackResults);

                            if (callbackResults.Success())
                            {
                                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance", "App Database Manager Instance Is Not Yet Initialized."));

                                if (callbackResults.Success())
                                {
                                    var databaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance").GetData();

                                    callbackResults.SetResult(databaseManagerInstance.GetAssetBundlesLibrary());

                                    if (callbackResults.Success())
                                    {
                                        var assetBundles = databaseManagerInstance.GetAssetBundlesLibrary().GetData();

                                        callbackResults.SetResult(assetBundles.GetDynamicContainer<DynamicContentContainer>(AppData.ScreenType.LandingPageScreen, AppData.ContentContainerType.SceneContentsContainer, AppData.ContainerViewSpaceType.Scene));

                                        if (callbackResults.Success())
                                        {
                                            var container = assetBundles.GetDynamicContainer<DynamicContentContainer>(AppData.ScreenType.LandingPageScreen, AppData.ContentContainerType.SceneContentsContainer, AppData.ContainerViewSpaceType.Scene).GetData();

                                            var clearContainerCallbackResultsTask = await container.ClearAsync(true, 1.0f);

                                            callbackResults.SetResult(clearContainerCallbackResultsTask);

                                            if (callbackResults.Success())
                                            {
                                                var postContent = GetPostContent(post).GetData();

                                                container.AddContent(postContent, false, true, true, contentAddedCallbackResults =>
                                                {
                                                    callbackResults.SetResult(contentAddedCallbackResults);
                                                });
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
                                else
                                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            }
                            else
                                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                        });
                    }
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance", "App Database Manager Instance Is Not Yet Initialized."));

                if(callbackResults.Success())
                {
                    var appDatabaseManagerInstance = AppData.Helpers.GetAppComponentValid(AppDatabaseManager.Instance, "App Database Manager Instance").GetData();

                    var loadContentAsyncCallbackResultsTask = await appDatabaseManagerInstance.GetPostContentAsync(post);
                    callbackResults.SetResult(loadContentAsyncCallbackResultsTask);

                    if(callbackResults.Success())
                    {
                        callbackResults.SetResult(GetPostContent(post));

                        if (callbackResults.Success())
                        {
                            SetPost(post, postSetCallbackResults =>
                            {
                                callbackResults.SetResult(postSetCallbackResults);

                            });
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

            callback?.Invoke(callbackResults);
        }

        public void AddPostWidget(AppData.SelectableWidget postWidget, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentValid(postWidget, "Post Widget", "Add Post Widget Failed - Post Widget Parameter Value Is Null -Invalid operation."));

            if(callbackResults.Success())
            {
                if(!postWidgets.Contains(postWidget))
                {
                    postWidgets.Add(postWidget);

                    if (postWidgets.Contains(postWidget))
                    {
                        callbackResults.result = $"Add Post Widget Success - Post Widget : {postWidget.GetName()} - Of Type : {postWidget.GetType().GetData()} Has Been Successfully Added to Post Widgets.";
                        callbackResults.resultCode = AppData.Helpers.SuccessCode;
                    }
                    else
                    {
                        callbackResults.result = $"Add Post Widget Failed With Error - Post Widget : {postWidget.GetName()} - Of Type : {postWidget.GetType().GetData()} Couldn't Be Added To Post Widgets For Some Unknown Reasons - Invalid operation -Please Check Here.";
                        callbackResults.resultCode = AppData.Helpers.ErrorCode;
                    }
                }
                else
                {
                    callbackResults.result = $"Add Post Widget Failed With Warning - Post Widget : {postWidget.GetName()} - Of Type : {postWidget.GetType().GetData()} Already Exists In Post Widgets - Invalid operation.";
                    callbackResults.resultCode = AppData.Helpers.WarningCode;
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void AddPostWidgets(Action<AppData.Callback> callback = null, params AppData.SelectableWidget[] postWidgetsParam)
        {
            var callbackResults = new AppData.Callback(AppData.Helpers.GetAppComponentsValid(postWidgetsParam, "Post Widgets", "Add Post Widgets Failed - Post Widgets Param Value Is Null -Invalid operation."));

            if (callbackResults.Success())
            {
                for (int i = 0; i < postWidgetsParam.Length; i++)
                {
                    if (!postWidgets.Contains(postWidgetsParam[i]))
                    {
                        postWidgets.Add(postWidgetsParam[i]);

                        if (postWidgets.Contains(postWidgetsParam[i]))
                        {
                            callbackResults.result = $"Add Post Widget Success - Post Widget : {postWidgetsParam[i].GetName()} - Of Type : {postWidgetsParam[i].GetType().GetData()} Has Been Successfully Added to Post Widgets.";
                            callbackResults.resultCode = AppData.Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.result = $"Add Post Widgets Failed With Error - Post Widget : {postWidgetsParam[i].GetName()} - Of Type : {postWidgetsParam[i].GetType().GetData()} Couldn't Be Added To Post Widgets For Some Unknown Reasons - Invalid operation -Please Check Here.";
                            callbackResults.resultCode = AppData.Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.result = $"Add Post Widgets Failed With Warning - Post Widget : {postWidgetsParam[i].GetName()} - Of Type : {postWidgetsParam[i].GetType().GetData()} Already Exists In Post Widgets - Invalid operation.";
                        callbackResults.resultCode = AppData.Helpers.WarningCode;
                    }
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void RemovePostWidget(AppData.SelectableWidget postWidget, Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetPostWidgets());

            if(callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(postWidget, "Post Widget", "Remove Post Widget Failed - Post Widget Parameter Value Is Null - Invalid Operation."));

                if (callbackResults.Success())
                {
                    if (GetPostWidgets().GetData().Contains(postWidget))
                    {
                        GetPostWidgets().GetData().Remove(postWidget);

                        if (!GetPostWidgets().GetData().Contains(postWidget))
                        {
                            callbackResults.result = $"Remove Post Widget Success - Post Widget : {postWidget.GetName()} - Of Type : {postWidget.GetType().GetData()} Has Been Successfully Removed From Post Widgets.";
                            callbackResults.resultCode = AppData.Helpers.SuccessCode;
                        }
                        else
                        {
                            callbackResults.result = $"Remove Post Widget Failed With Error - Post Widget : {postWidget.GetName()} - Of Type : {postWidget.GetType().GetData()} Couldn't Be Removed For Some Unknown Reasons- Invalid operation - Please Check Here.";
                            callbackResults.resultCode = AppData.Helpers.ErrorCode;
                        }
                    }
                    else
                    {
                        callbackResults.result = $"Remove Post Widget Failed With Warning - Post Widget : {postWidget.GetName()} - Of Type : {postWidget.GetType().GetData()} Doesn't Exists In Post Widgets - Invalid operation.";
                        callbackResults.resultCode = AppData.Helpers.WarningCode;
                    }
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void RemovePostWidgets(Action<AppData.Callback> callback = null, params AppData.SelectableWidget[] postWidgets)
        {
            var callbackResults = new AppData.Callback(GetPostWidgets());

            if(callbackResults.Success())
            {
                callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(postWidgets, "Post Widgets", "Remove Post Widgets Failed - Post Widgets Param Value Is Null - Invalid Operation."));

                if (callbackResults.Success())
                {
                    for (int i = 0; i < postWidgets.Length; i++)
                    {
                        RemovePostWidget(postWidgets[i], widgetRemovedCallbackResults => 
                        {
                            callbackResults.SetResult(widgetRemovedCallbackResults);
                        });

                        if (callbackResults.UnSuccessful())
                        {
                            Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                            break;
                        }
                    }
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            callback?.Invoke(callbackResults);
        }

        public void RemovePostWidgets(Action<AppData.Callback> callback = null)
        {
            var callbackResults = new AppData.Callback(GetPostWidgets());

            if (callbackResults.Success())
            {
                GetPostWidgets().GetData().Clear();

                if(GetPostWidgets().GetData().Count <= 0)
                {
                    callbackResults.result = $"Remove Post Widgets Success - {GetPostWidgets().GetData().Count} Post Widget(s) Have Been Removed Successfully.";
                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                }
                else
                {
                    callbackResults.result = $"Remove Post Widgets Failed With Error - Couldn't Remove : {GetPostWidgets().GetData().Count} Post Widget(s) For Some Unknown Reasons- Invalid operation - Please Check Here.";
                    callbackResults.resultCode = AppData.Helpers.ErrorCode;
                }
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

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


        public AppData.CallbackData<AppData.SelectableWidget> GetPostWidget(AppData.Post post)
        {
            var callbackResults = new AppData.CallbackData<AppData.SelectableWidget>(GetPostWidgets());

            if(callbackResults.Success())
            {
                var postWidget = GetPostWidgets().GetData().Find(postWidget => postWidget.GetName() == post.GetName());

                callbackResults.SetResult(AppData.Helpers.GetAppComponentValid(postWidget, "Post Widget", $"Failed To Find Post Widget For :{post.GetName()} - Invalid Operation."));

                if(callbackResults.Success())
                {
                    callbackResults.result = $"Post Widget For :{post.GetName()} - Has Been Successfully Found.";
                    callbackResults.data = postWidget;
                }
                else
                    Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
            }
            else
                Log(callbackResults.GetResultCode, callbackResults.GetResult, this);

            return callbackResults;
        }

        public AppData.CallbackDataList<AppData.SelectableWidget> GetPostWidgets()
        {
            var callbackResults = new AppData.CallbackDataList<AppData.SelectableWidget>();

            callbackResults.SetResult(AppData.Helpers.GetAppComponentsValid(postWidgets, "Post Widgets", "Get Post Widgets Failed - There Are No Post Widgets Found - Invalid Operation."));

            if(callbackResults.Success())
            {
                callbackResults.result = $"Get Post Widgets Success - {postWidgets.Count} Post Widget(s) Have Been Found.";
                callbackResults.data = postWidgets;
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
                                {
                                    screen.HideScreenWidget(AppData.WidgetType.LoadingWidget);

                                    callbackResults.SetResult(GetPostWidget(postSelectedCallbacKResults.GetData()));

                                    if(callbackResults.Success())
                                    {
                                        var widget = GetPostWidget(postSelectedCallbacKResults.GetData()).GetData();

                                        widget.Select(widgetSelectedCallbackResults => 
                                        {
                                            callbackResults.SetResult(widgetSelectedCallbackResults);
                                        });
                                    }
                                    else
                                        Log(callbackResults.GetResultCode, callbackResults.GetResult, this);
                                }
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