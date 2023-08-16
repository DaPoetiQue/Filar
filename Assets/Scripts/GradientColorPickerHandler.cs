using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Com.RedicalGames.Filar
{
    public class GradientColorPickerHandler : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        #region Components

        [SerializeField]
        RectTransform gradientSelectorInput = null;

        [Space(5)]
        [SerializeField]
        AppData.UIScreenDimensions gradientResolution = new AppData.UIScreenDimensions();

        [SerializeField]
        List<AppData.ColorData> colors = new List<AppData.ColorData>();

        [Space(5)]
        [SerializeField]
        RawImage saturationGradientDisplayer = null;

        RawImage gradientDisplayer = null;
        RectTransform gradientInputContainerRect;
        Texture2D gradientColoredBG;

        AppData.ColorInfo selectedColorInfo = new AppData.ColorInfo();

        bool isInitialized;
        bool colorsGenerated = false;

        Rect gradientPalletRect;

        Dictionary<Color, AppData.ColorData> gradientColorsLibrary = new Dictionary<Color, AppData.ColorData>();

        Color[] gradientColors;

        Texture2D tex;
        Texture2D gradient;

        #endregion

        #region Unity Callbacks

        void Start() => Init();

        #endregion

        #region Main

        void Init()
        {
            isInitialized = IsInitialized();
        }

        bool IsInitialized()
        {
            if (gradientSelectorInput == null)
                return false;

            if (gradientInputContainerRect == null)
            {
                gradientInputContainerRect = GetComponent<RectTransform>();
                gradientPalletRect = gradientInputContainerRect.rect;
                gradientDisplayer = GetComponent<RawImage>();
            }

            GenerateColorData((initializeCallBackResults) =>
            {
                if (AppData.Helpers.IsSuccessCode(initializeCallBackResults.resultCode))
                {
                    if (gradientColoredBG == null)
                    {
                        gradientColoredBG = (Texture2D)gradientInputContainerRect.GetComponent<RawImage>().texture;

                        if (!gradientColoredBG.isReadable)
                            Debug.LogError("----------------> Gradient BG Is Not Readable - Set Readable In The Editor Inspector.");
                    }
                }
                else
                    Debug.LogError($"--> Initialize Color Data Failed With Results : {initializeCallBackResults.result}");
            });

            return gradientInputContainerRect && gradientSelectorInput && gradientColoredBG;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isInitialized)
            {
                Vector2 pos;

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(gradientInputContainerRect, eventData.position, eventData.enterEventCamera, out pos))
                {
                    if (pos.x > gradientPalletRect.x && pos.y > gradientPalletRect.y && pos.x < (gradientPalletRect.width + gradientPalletRect.x) && pos.y < (gradientPalletRect.height + gradientPalletRect.y))
                    {
                        int x = Mathf.Clamp(0, (int)(((pos.x - gradientPalletRect.x) * gradientColoredBG.width) / gradientPalletRect.width), gradientColoredBG.width);
                        int y = Mathf.Clamp(0, (int)(((pos.y - gradientPalletRect.y) * gradientColoredBG.height) / gradientPalletRect.height), gradientColoredBG.height);

                        Color selectedColor = gradientColoredBG.GetPixel(x, y);

                        DatabaseManager.Instance.GetHexidecimalFromColor(selectedColor, (getHexadecimalCallbackResults) =>
                        {
                            if (AppData.Helpers.IsSuccessCode(getHexadecimalCallbackResults.resultCode))
                            {
                                selectedColorInfo = getHexadecimalCallbackResults.data;
                                AppData.ActionEvents.OnSwatchColorPickedEvent(selectedColorInfo, true, false);

                                gradientSelectorInput.anchoredPosition = pos;
                            }
                            else
                                Debug.LogWarning($"--> GradientColorPickerHandler OnPointerDown Failed With Results : {getHexadecimalCallbackResults.result}");
                        });
                    }
                }
                else
                    return;
            }
            else
                Debug.LogWarning("--> GradientColorPickerHandler OnPointerDown Failed : Components Missings");
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isInitialized)
            {
                Vector2 pos;

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(gradientInputContainerRect, eventData.position, eventData.enterEventCamera, out pos))
                {
                    if (pos.x > gradientPalletRect.x && pos.y > gradientPalletRect.y && pos.x < (gradientPalletRect.width + gradientPalletRect.x) && pos.y < (gradientPalletRect.height + gradientPalletRect.y))
                    {
                        int x = Mathf.Clamp(0, (int)(((pos.x - gradientPalletRect.x) * gradientColoredBG.width) / gradientPalletRect.width), gradientColoredBG.width);
                        int y = Mathf.Clamp(0, (int)(((pos.y - gradientPalletRect.y) * gradientColoredBG.height) / gradientPalletRect.height), gradientColoredBG.height);

                        Color selectedColor = gradientColoredBG.GetPixel(x, y);

                        DatabaseManager.Instance.GetHexidecimalFromColor(selectedColor, (getHexadecimalCallbackResults) =>
                        {
                            if (AppData.Helpers.IsSuccessCode(getHexadecimalCallbackResults.resultCode))
                            {
                                selectedColorInfo = getHexadecimalCallbackResults.data;
                                AppData.ActionEvents.OnSwatchColorPickedEvent(selectedColorInfo, true, false);

                                gradientSelectorInput.anchoredPosition = pos;
                            }
                            else
                                Debug.LogWarning($"--> GradientColorPickerHandler OnDrag Failed With Results : {getHexadecimalCallbackResults.result}");
                        });
                    }
                }
                else
                    return;
            }
            else
                Debug.LogWarning("--> GradientColorPickerHandler OnDrag Failed : Components Missings");
        }

        public void UpdateGradientInput(AppData.ColorInfo colorInfo)
        {
            if (colorsGenerated)
            {
                AppData.ColorData data = colors.Find((x) => x.color == colorInfo.color);

                if (data != null)
                {
                    Debug.LogError("--> UpdateGradientInput Success : Color Found");
                }
                else
                {
                    Debug.LogError("--> UpdateGradientInput Failed : Color Not Found");
                }
            }
            else
                Debug.LogError("--> UpdateGradientInput Failed : Colors Not Generated");
        }

        void GenerateColorData(Action<AppData.CallbackData<Texture2D>> callback)
        {
            AppData.CallbackData<Texture2D> callbackResults = new AppData.CallbackData<Texture2D>();

            if (gradientDisplayer)
            {
                int pixelCount = gradientResolution.width * gradientResolution.height;
                gradientColors = DatabaseManager.Instance.GetColorSpectrum(pixelCount);

                if (tex == null)
                    tex = new Texture2D(gradientResolution.width, gradientResolution.height, TextureFormat.RGBA32, true, true);

                if (gradient == null)
                    gradient = new Texture2D(gradientResolution.width, gradientResolution.height, TextureFormat.RGBA32, true, true);

                tex.SetPixels(gradientColors);
                tex.Apply();

                gradientDisplayer.texture = tex;

                if (gradientDisplayer.texture == tex)
                {
                    callbackResults.result = "Success";
                    callbackResults.data = tex;
                    callbackResults.resultCode = AppData.Helpers.SuccessCode;
                }

                if (saturationGradientDisplayer)
                {
                    for (int x = 0; x < gradientResolution.width; x++)
                    {
                        for (int y = 0; y < gradientResolution.height; y++)
                        {
                            var r = 255 - y;
                            var g = 255 - x - y;
                            var b = 255 - x - y;

                            gradient.SetPixel(x, y, new Color(r, g, b));
                        }
                    }

                    gradient.Apply();

                    saturationGradientDisplayer.texture = gradient;
                }
            }
            else
            {
                callbackResults.result = "Setting Gradient Displayer Failed : Displayer Not Found";
                callbackResults.data = default;
                callbackResults.resultCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);

            //for (int x = 0; x < gradientColoredBG.width; x++)
            //{
            //    for (int y = 0; y < gradientColoredBG.height; y++)
            //    {
            //        Color color = gradientColoredBG.GetPixel(x, y);

            //        if (!gradientColorsLibrary.ContainsKey(color))
            //        {
            //            AppData.ColorData data = new AppData.ColorData();
            //            data.color = color;
            //            data.coordinates = new AppData.ScreenCoordinates(x, y);

            //            gradientColorsLibrary.Add(color, data);

            //            colors.Add(data);
            //        }
            //        else
            //            Debug.LogError("--> Color Already Exists.");
            //    }
            //}

            //colorsGenerated = gradientColorsLibrary.Count > 0;
        }

        public bool ColorExistsInGradientSpectrum()
        {
            return false;
        }

        #endregion
    }
}
