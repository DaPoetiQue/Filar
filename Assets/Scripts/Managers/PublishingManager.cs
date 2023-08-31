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

        public async void OnPublish(AppData.PostHandler postHandler, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (postHandler != null)
            {
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

                while (databaseReference == null)
                    await Task.Yield();

                await Task.Delay(1000);

                FirebaseDatabase.DefaultInstance.GetReference("App Info").ValueChanged += PublishingManager_ValueChanged;
                FirebaseDatabase.DefaultInstance.GetReference("Posts").ValueChanged += PublishingManager_ValueChanged;

                // var postData = JsonConvert.SerializeObject(post, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

                var post = JsonUtility.ToJson(postHandler.post);
                var postProfileData = JsonUtility.ToJson(postHandler.profile);
                var postContentData = JsonUtility.ToJson(postHandler.content);

                LogInfo($" Post Identifier : {postHandler.GetIdentifier()} - Post Data : {postProfileData}", this);

                Dictionary<string, object> postObject = new Dictionary<string, object>();
                postObject.Add(postHandler.GetIdentifier(), post);

                Dictionary<string, object> postProfileDataObject = new Dictionary<string, object>();
                postProfileDataObject.Add(postHandler.GetIdentifier(), postProfileData);

                Dictionary<string, object> postContentObject = new Dictionary<string, object>();
                postContentObject.Add(postHandler.GetIdentifier(), postContentData);

                await databaseReference.Child("Posts Runtime Data").Child("Posts").UpdateChildrenAsync(postObject);
                await databaseReference.Child("Posts Runtime Data").Child("Profiles").UpdateChildrenAsync(postProfileDataObject);
                await databaseReference.Child("Posts Runtime Data").Child("Contents").UpdateChildrenAsync(postContentObject);

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