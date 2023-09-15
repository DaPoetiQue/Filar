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

        [SerializeField]
        Texture2D[] images;

        #endregion

        #region Main

        public async void Publish()
        {
            #region Post Handler

            //string profileIdentifier = AppData.Helpers.GenerateAppKey(5); // This Will Come Publishing User Profile Identifier
            string postIdentifier = AppData.Helpers.GenerateUniqueIdentifier(5);
            string contentIdentifier = AppData.Helpers.GenerateUniqueIdentifier(5);

            string profileIdentifier = "17G6 16S6 27I4 20K3 67M7";

            #endregion

            #region Post

           post.SetUniqueIdentifier(postIdentifier);
            post.SetRootdentifier(profileIdentifier);
            post.InitializeCreationDateTime();

            #endregion

            #region Content

            //var results = await AppData.Helpers.GetMeshDataAsync(obj);

            AppData.ContentGenerator contentGenerator = new AppData.ContentGenerator(obj);

            //content.SetUniqueIdentifier(contentIdentifier);
            //content.SetRootdentifier(postIdentifier);

            var getObjectStringTaskResults = await contentGenerator.SetGameObject();

            if (getObjectStringTaskResults.Success())
            {
                AppData.SerializableGameObject content = new AppData.SerializableGameObject(getObjectStringTaskResults.data);

                AppData.SerializableImage thumbnailData = new AppData.SerializableImage(thumbnail, imageEncoderType);

                Log(getObjectStringTaskResults.ResultCode, getObjectStringTaskResults.Result, this);

                //var path = Path.Combine(Application.streamingAssetsPath, $"{post.GetTitle()}.json").Replace("\\", "/");
                //File.WriteAllText(path, getObjectStringTaskResults.data);


                #region Publish

                PublishingManager.Instance.OnPublish(post, thumbnailData, content, contentIdentifier, publishCallbackResults =>
                {
                    Log(publishCallbackResults.ResultCode, publishCallbackResults.Result, this);
                });


                #endregion
            }

            #endregion
        }

        public void Upload()
        {
            AppData.SerializableImage imagesData = new AppData.SerializableImage(encoderType: imageEncoderType, images: images);

            #region Upload

            LogInfo($"Publishing : {images.Length} Images With Data Count : {imagesData.GetImageDataFromCompressedData().Length}", this);

            PublishingManager.Instance.OnUploadImageData(imagesData, uploadCallbackResults =>
            {
                Log(uploadCallbackResults.ResultCode, uploadCallbackResults.Result, this);
            });

            #endregion
        }

        #endregion
    }
}