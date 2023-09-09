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

        [SerializeField]
        string postURL = "User Post Runtime Content";

        [Space(5)]
        [SerializeField]
        string postContentsURL = "User Post Data";

        public string PostURL { get { return postURL; } private set { } }
        public string PostContentsURL { get { return postContentsURL; } private set { } }


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

                databaseReference.Database.GetReference(postURL).ValueChanged += PublishingManager_ValueChanged;

                var postData = JsonUtility.ToJson(post);

                string postKey = post.GetUniqueIdentifier();

                Dictionary<string, object> postObject = new Dictionary<string, object>();
                postObject.Add(postKey, postData);

                Dictionary<string, object> postContentObject = new Dictionary<string, object>();

                if (content.GetMeshBytesArray() != null && content.GetMeshBytesArray().Length > 0)
                {
                    await storageReference.Child(postContentsURL).Child(post.GetRootIdentifier()).Child(postKey).Child("Model").PutBytesAsync(content.GetMeshBytesArray());

                    if (thumbnailData.GetImageDataFromCompressedData() != null && thumbnailData.GetImageDataFromCompressedData().Length > 0)
                        await storageReference.Child(postContentsURL).Child(post.GetRootIdentifier()).Child(postKey).Child("Thumbnail").PutBytesAsync(thumbnailData.GetImageDataFromCompressedData());
                    else
                        LogError("Thumbnail Data Not Assigned", this);
                }
                else
                {
                    sw.Stop();
                    LogError($"Failed To Published Post With :{content.GetMeshStringList().Count} Meshes In : {sw.ElapsedMilliseconds / 1000} Seconds", this);
                }

                await databaseReference.Child("Posts Runtime Data").Child("Post Info Database").UpdateChildrenAsync(postObject);

                sw.Stop();

                LogSuccess($"Post : {postKey} Has Been Published Successfully In : {sw.ElapsedMilliseconds / 1000} Seconds", this);
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
