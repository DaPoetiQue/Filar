using Firebase.Database;
using Firebase.Storage;
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
        StorageReference storageReference;

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

        public async void OnPublish(AppData.Post post, AppData.SerializableImage thumbnailData, AppData.SerializableGameObject content, string postContentKey, Action<AppData.Callback> callback = null)
        {
            AppData.Callback callbackResults = new AppData.Callback();

            if (post != null)
            {
                var sw = new System.Diagnostics.Stopwatch();
                sw.Start();

                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                storageReference = FirebaseStorage.DefaultInstance.RootReference;

                while (databaseReference == null)
                    await Task.Yield();

                await Task.Delay(1000);

                databaseReference.Database.GetReference("User Post Runtime Content").ValueChanged += PublishingManager_ValueChanged;

                var postData = JsonUtility.ToJson(post);

                string postKey = post.GetUniqueIdentifier();

                Dictionary<string, object> postObject = new Dictionary<string, object>();
                postObject.Add(postKey, postData);

                await databaseReference.Child("Posts Runtime Data").Child("Post Info Database").UpdateChildrenAsync(postObject);

                Dictionary<string, object> postContentObject = new Dictionary<string, object>();

                if (content.GetMeshBytesArray() != null && content.GetMeshBytesArray().Length > 0)
                {
                    await storageReference.Child("User Post Data").Child(post.GetRootIdentifier()).Child(postKey).Child("Model").PutBytesAsync(content.GetMeshBytesArray());

                    if (thumbnailData.GetImageData() != null && thumbnailData.GetImageData().Length > 0)
                        await storageReference.Child("User Post Data").Child(post.GetRootIdentifier()).Child(postKey).Child("Thumbnail").PutBytesAsync(thumbnailData.GetImageData());
                    else
                        LogError("Thumbnail Data Not Assigned", this);

                    sw.Stop();

                    LogSuccess($"Post With : {content.GetMeshStringList().Count} Meshes Has Been Published Successfully In : {sw.ElapsedMilliseconds / 1000} Seconds", this);
                }
                else
                {
                    sw.Stop();
                    LogError($"Failed To Published Post With :{content.GetMeshStringList().Count} Meshes In : {sw.ElapsedMilliseconds / 1000} Seconds", this);
                }
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
