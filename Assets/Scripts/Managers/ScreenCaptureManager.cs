using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class ScreenCaptureManager : AppData.SingletonBaseComponent<ScreenCaptureManager>
    {
        #region Components

        [SerializeField]
        AppData.FileExtensionType captureFileExtension = AppData.FileExtensionType.JPG;

        [Space(5)]
        [SerializeField]
        bool cropImage = false;

        [Space(5)]
        [SerializeField]
        AppData.UIScreenDimensions captureResolution = new AppData.UIScreenDimensions();

        [Space(5)]
        [SerializeField]
        AppData.ScreenCoordinates captureScreenCoordinates = new AppData.ScreenCoordinates();

        [Space(5)]
        [SerializeField]
        List<AppData.UIHidableScreenContent> hidableUIScreenContentList = new List<AppData.UIHidableScreenContent>();

        [Space(5)]
        [SerializeField]
        AppData.UIHidableScreenContent waterMarkObject = new AppData.UIHidableScreenContent();


        AppData.ImageData captureData = new AppData.ImageData();

        Coroutine screenCaptureRoutine;

        Texture2D croppedTexture;

        #endregion

        #region Unity Callbacks

        void Start() => Init();

        #endregion

        #region Main

        void Init()
        {
            croppedTexture = new Texture2D(captureResolution.width, captureResolution.height);

            if (waterMarkObject.value)
                waterMarkObject.Hide();
        }

        public void CaptureScreen(Action<AppData.CallbackData<AppData.ImageData>> callback)
        {
            if (screenCaptureRoutine != null)
            {
                StopCoroutine(screenCaptureRoutine);
                screenCaptureRoutine = null;
            }

            screenCaptureRoutine = StartCoroutine(OnScreenCaptureAsync((screenCapture) =>
            {
                if (AppData.Helpers.IsSuccessCode(screenCapture.resultCode))
                    captureData = screenCapture.data;

                callback?.Invoke(screenCapture);
            }));
        }

        public AppData.ImageData GetScreenCaptureData()
        {
            return captureData;
        }

        IEnumerator OnScreenCaptureAsync(Action<AppData.CallbackData<AppData.ImageData>> callback)
        {
            AppData.CallbackData<AppData.ImageData> callbackResults = new AppData.CallbackData<AppData.ImageData>();

            if (hidableUIScreenContentList.Count > 0)
                foreach (var hidable in hidableUIScreenContentList)
                    hidable.Hide();

            if (waterMarkObject.value)
                waterMarkObject.Show();

            yield return new WaitForEndOfFrame();

            Texture2D capturedTexture = ScreenCapture.CaptureScreenshotAsTexture();

            if (waterMarkObject.value)
                waterMarkObject.Hide();

            if (hidableUIScreenContentList.Count > 0)
                foreach (var hidable in hidableUIScreenContentList)
                    hidable.Show();

            if (capturedTexture != null)
            {
                if (cropImage)
                {
                    croppedTexture.SetPixels(capturedTexture.GetPixels(captureScreenCoordinates.x,
                        captureScreenCoordinates.y, captureResolution.width, captureResolution.height));

                    croppedTexture.Apply();
                }

                AppData.ImageData captureData = new AppData.ImageData();

                captureData.texture = (cropImage) ? croppedTexture : capturedTexture;
                captureData.extensionType = captureFileExtension;

                switch (captureFileExtension)
                {
                    case AppData.FileExtensionType.JPG:

                        captureData.data = (cropImage) ? croppedTexture.EncodeToJPG() : capturedTexture.EncodeToJPG();

                        break;

                    case AppData.FileExtensionType.PNG:

                        captureData.data = (cropImage) ? croppedTexture.EncodeToPNG() : capturedTexture.EncodeToPNG();

                        break;
                }

                AppData.ActionEvents.OnPlayAudioEvent(AppData.AudioType.CameraShutter);

                callbackResults.result = "Success : Screen Captured.";
                callbackResults.data = captureData;
                callbackResults.resultCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.result = "Failed : Screen Captured Texture Is Null.";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        #endregion
    }
}
