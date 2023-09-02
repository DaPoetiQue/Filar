using System;
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
            AppData.ModelMeshData content = new AppData.ModelMeshData();

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

            var results = await AppData.Helpers.GetMeshDataAsync(obj);

            content.SetUniqueIdentifier(contentIdentifier);
            content.SetRootdentifier(postIdentifier);
            content.mesh = content.MeshDataToString(results.data, "m|");

            #endregion

            #region Publish

            PublishingManager.Instance.OnPublish(post, content, publishCallbackResults => 
            {
                Log(publishCallbackResults.ResultCode, publishCallbackResults.Result, this);
            });

            #endregion
        }

        #endregion
    }
}