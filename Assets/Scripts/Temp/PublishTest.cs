using System;
using System.IO;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class PublishTest : AppMonoBaseClass
    {
        #region Components

        [SerializeField]
        AppData.Post post = new AppData.Post();

        [SerializeField]
        GameObject obj;

        [SerializeField]
        Sprite thumbnail;

        [SerializeField]
        AppData.Helpers.ImageEncoderType imageEncoderType;

        #endregion

        #region Main

        public async void Publish()
        {
            #region Post Handler

            string profileIdentifier = AppData.Helpers.GenerateAppKey(5); // This Will Come Publishing User Profile Identifier
            string postIdentifier = AppData.Helpers.GenerateUniqueIdentifier(5);
            string contentIdentifier = AppData.Helpers.GenerateUniqueIdentifier(5);

            #endregion

            #region Post

            post.SetUniqueIdentifier(postIdentifier);
            post.SetRootdentifier(profileIdentifier);
            post.InitializeCreationDateTime();
            post.SetThumbnail(thumbnail, imageEncoderType);

            #endregion

            #region Content

            //var results = await AppData.Helpers.GetMeshDataAsync(obj);

            AppData.ContentGenerator contentGenerator = new AppData.ContentGenerator(obj);

            //content.SetUniqueIdentifier(contentIdentifier);
            //content.SetRootdentifier(postIdentifier);

            var getObjectStringTaskResults = await contentGenerator.GameObjectToBytesArray(compressionLevel: System.IO.Compression.CompressionLevel.Optimal);

            if (getObjectStringTaskResults.Success())
            {
                AppData.SerializableGameObject content = new AppData.SerializableGameObject(getObjectStringTaskResults.data);

                Log(getObjectStringTaskResults.ResultCode, getObjectStringTaskResults.Result, this);

                //var path = Path.Combine(Application.streamingAssetsPath, $"{post.GetTitle()}.json").Replace("\\", "/");
                //File.WriteAllText(path, getObjectStringTaskResults.data);

                #endregion

                #region Publish

                PublishingManager.Instance.OnPublish(post, content, contentIdentifier, publishCallbackResults =>
                {
                    Log(publishCallbackResults.ResultCode, publishCallbackResults.Result, this);
                });
            }

            #endregion
        }

        #endregion
    }
}