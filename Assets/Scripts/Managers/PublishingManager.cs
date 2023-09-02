using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace Com.RedicalGames.Filar
{
    public class PublishingManager : AppMonoBaseClass
    {
        #region Static

        private static PublishingManager _instance;

        public static PublishingManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<PublishingManager>();

                return _instance;
            }
        }


        #endregion

        #region Components

        DatabaseReference databaseReference;

        #endregion

        #region Unity Callbacks

        #endregion

        #region Main

        public void Publish(Action<AppData.Callback> callback = null)
        {
            //AppData.Callback callbackResults = new AppData.Callback();

            //if (networkRoutine != null)
            //    StopCoroutine(networkRoutine);

            //networkRoutine = StartCoroutine(CheckNetworkStatus());

            //if (callback != null)
            //    callback.Invoke(callbackResults);
        }

        public async void OnPublish(AppData.Post post, AppData.ModelMeshData content, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (post != null)
            {
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

                while (databaseReference == null)
                    await Task.Yield();

                await Task.Delay(1000);

                FirebaseDatabase.DefaultInstance.GetReference("Posts Runtime Data").ValueChanged += PublishingManager_ValueChanged;

                var postData = JsonUtility.ToJson(post);
                var postContentData = JsonUtility.ToJson(content);

                string postKey = post.GetTitle() + $"_{post.GetUniqueIdentifier()}";
                string contentKey = post.GetTitle() + $"_{content.GetUniqueIdentifier()}";

                Dictionary<string, object> postObject = new Dictionary<string, object>();
                postObject.Add(postKey, postData);

                Dictionary<string, object> postContentObject = new Dictionary<string, object>();
                postContentObject.Add(contentKey, postContentData);

                await databaseReference.Child("Posts Runtime Data").Child("Post Info Database").UpdateChildrenAsync(postObject);
                await databaseReference.Child("Posts Runtime Data").Child("Post Content Database").UpdateChildrenAsync(postContentObject);

                sw.Stop();
                LogSuccess($" Post Published Successfully In : {sw.ElapsedMilliseconds / 1000} Seconds", this);
            }
            else
                LogError("Post Is Missing - Null", this);

            callback?.Invoke(callbackResults);
        }

        private void PublishingManager_ValueChanged1(object sender, ValueChangedEventArgs e)
        {
      
        }

        private void PublishingManager_ValueChanged(object sender, ValueChangedEventArgs e)
        {
           
        }

        #endregion
    }
}
