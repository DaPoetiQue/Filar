using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Com.RedicalGames.Filar
{
    public class ColorDropPickerHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        #region Components

        [SerializeField]
        RectTransform safeAreaRectTransform = null;

        [Space(5)]
        [SerializeField]
        ColorPalletWidgetUIHandler widgetUIHandler = null;

        ColorPickerVisualizer colorPickerVisualizer = null;
        RectTransform rectTransform;
        GraphicRaycaster graphicRaycaster;

        Coroutine captureScreenInfoRoutine;

        Rect screenDataRect;
        Texture2D screenDataTexture;

        AppData.ColorInfo selectedColorInfo = new AppData.ColorInfo();

        #endregion

        #region Main

        public void Init(RectTransform safeAreaRectTransform = null)
        {
            if (safeAreaRectTransform != null)
                this.safeAreaRectTransform = safeAreaRectTransform;

            graphicRaycaster = this.GetComponent<GraphicRaycaster>();

            colorPickerVisualizer = GetComponentInChildren<ColorPickerVisualizer>();

            if (colorPickerVisualizer)
                colorPickerVisualizer.Initialize();
            else
                Debug.LogWarning("--> Failed - Color Picker Visualizer Is not Found In Childer.");

            rectTransform = this.GetComponent<RectTransform>();
            screenDataRect = rectTransform.rect;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(safeAreaRectTransform, eventData.position, eventData.enterEventCamera))
                if (widgetUIHandler)
                {
                    widgetUIHandler.UpdateInputActionCheckbox(AppData.CheckboxInputActionType.ToggleColorDropPicker, true);
                    return;
                }

            Vector2 pos;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out pos))
            {
                colorPickerVisualizer.ShowPicker();

                if (screenDataTexture != null)
                {
                    if (pos.x > screenDataRect.x && pos.y > screenDataRect.y && pos.x < (screenDataRect.width + screenDataRect.x) && pos.y < (screenDataRect.height + screenDataRect.y))
                    {
                        int x = Mathf.Clamp(0, (int)(((pos.x - screenDataRect.x) * screenDataTexture.width) / screenDataRect.width), screenDataTexture.width);
                        int y = Mathf.Clamp(0, (int)(((pos.y - screenDataRect.y) * screenDataTexture.height) / screenDataRect.height), screenDataTexture.height);

                        selectedColorInfo.color = screenDataTexture.GetPixel(x, y);
                        colorPickerVisualizer.SetPickedColor(selectedColorInfo.color);

                        SceneAssetsManager.Instance.GetHexidecimalFromColor(selectedColorInfo.color, (getHexadecimalCallbackResults) =>
                        {
                            if (AppData.Helpers.IsSuccessCode(getHexadecimalCallbackResults.resultsCode))
                            {
                                selectedColorInfo = getHexadecimalCallbackResults.data;
                                AppData.ActionEvents.OnSwatchColorPickedEvent(selectedColorInfo, true, false);

                                colorPickerVisualizer.SetPickerScreenPosition(pos);
                            }
                            else
                                Debug.LogWarning($"--> GradientColorPickerHandler OnPointerDown Failed With Results : {getHexadecimalCallbackResults.results}");
                        });
                    }
                }
                else
                    Debug.LogWarning("--> OnDrag Failed : screenDataTexture Missing / Null.");
            }
        }


        public void OnDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(safeAreaRectTransform, eventData.position, eventData.enterEventCamera))
                return;

            Vector2 pos;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out pos))
            {
                colorPickerVisualizer.ShowPicker();

                if (screenDataTexture != null)
                {
                    if (pos.x > screenDataRect.x && pos.y > screenDataRect.y && pos.x < (screenDataRect.width + screenDataRect.x) && pos.y < (screenDataRect.height + screenDataRect.y))
                    {
                        int x = Mathf.Clamp(0, (int)(((pos.x - screenDataRect.x) * screenDataTexture.width) / screenDataRect.width), screenDataTexture.width);
                        int y = Mathf.Clamp(0, (int)(((pos.y - screenDataRect.y) * screenDataTexture.height) / screenDataRect.height), screenDataTexture.height);

                        selectedColorInfo.color = screenDataTexture.GetPixel(x, y);
                        colorPickerVisualizer.SetPickedColor(selectedColorInfo.color);

                        SceneAssetsManager.Instance.GetHexidecimalFromColor(selectedColorInfo.color, (getHexadecimalCallbackResults) =>
                        {
                            if (AppData.Helpers.IsSuccessCode(getHexadecimalCallbackResults.resultsCode))
                            {
                                selectedColorInfo = getHexadecimalCallbackResults.data;
                                AppData.ActionEvents.OnSwatchColorPickedEvent(selectedColorInfo, true, false);

                                colorPickerVisualizer.SetPickerScreenPosition(pos);
                            }
                            else
                                Debug.LogWarning($"--> GradientColorPickerHandler OnPointerDown Failed With Results : {getHexadecimalCallbackResults.results}");
                        });
                    }
                }
                else
                    Debug.LogWarning("--> OnDrag Failed : screenDataTexture Missing / Null.");
            }
        }

        public void OnPointerUp(PointerEventData eventData) => colorPickerVisualizer.HidePicker();

        public void Enable()
        {
            graphicRaycaster.enabled = true;

            AppData.ActionEvents.OnScreenViewStateChangedEvent(AppData.ScreenViewState.Overlayed);

            if (captureScreenInfoRoutine != null)
                captureScreenInfoRoutine = null;

            captureScreenInfoRoutine = StartCoroutine(CaptureScreenInfo((captureResults) =>
            {
                if (AppData.Helpers.IsSuccessCode(captureResults.resultsCode))
                {
                    screenDataTexture = captureResults.data;
                    Debug.LogError($"--> Capture Screen Info Success : {captureResults.data.GetPixels().Length} Pixels Found.");
                }
                else
                    Debug.LogWarning($"--> On Enable CaptureScreenInfo Failed With Results : {captureResults.results}");
            }));


        }

        public void Disable()
        {
            graphicRaycaster.enabled = false;

            AppData.ActionEvents.OnScreenViewStateChangedEvent(AppData.ScreenViewState.Focused);
        }

        IEnumerator CaptureScreenInfo(Action<AppData.CallbackData<Texture2D>> callback)
        {
            AppData.CallbackData<Texture2D> callbackResults = new AppData.CallbackData<Texture2D>();

            yield return new WaitForEndOfFrame();

            Texture2D screenColorData = ScreenCapture.CaptureScreenshotAsTexture();

            yield return new WaitForEndOfFrame();

            if (screenColorData)
            {
                callbackResults.results = "Screen Color Data Pixels Loaded Successfully.";
                callbackResults.data = screenColorData;
                callbackResults.resultsCode = AppData.Helpers.SuccessCode;
            }
            else
            {
                callbackResults.results = "Screen Color Data Not Loaded.";
                callbackResults.data = default;
                callbackResults.resultsCode = AppData.Helpers.ErrorCode;
            }

            callback?.Invoke(callbackResults);
        }

        #endregion
    }
}
