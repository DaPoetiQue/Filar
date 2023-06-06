using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]

public class ColorPickerVisualizer : MonoBehaviour
{
    #region Components

    [SerializeField]
    float transitionSpeed = 5.0f;

    [Space(5)]
    [SerializeField]
    Image pickedColorDisplayer;

    CanvasGroup canvasGroup;

    RectTransform pickerTransform;

    bool canTransition = false;
    bool show;

    #endregion

    #region Unity Callbacks

    void Update() => OnColorPickerTransition();

    #endregion

    #region Main

    public void Initialize()
    {
        if (GetComponent<CanvasGroup>())
            canvasGroup = GetComponent<CanvasGroup>();
        else
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        pickerTransform = this.GetComponent<RectTransform>();

        canvasGroup.alpha = 0.0f;

        if (pickedColorDisplayer == null)
            Debug.LogWarning("--> ColorPickerVisualizer Initialize Failed : Picked Color Displayer Missing / Null.");
    }

    public void ShowPicker()
    {
        show = true;
        canTransition = true;
    }

    public void HidePicker()
    {
        show = false;
        canTransition = true;
    }

    public void SetPickerScreenPosition(Vector2 position) => pickerTransform.anchoredPosition = position;

    public void SetPickedColor(Color color) 
    {
        pickedColorDisplayer.color = color;

        Debug.LogError($"--> Set Color - R : {color.r} - G : {color.g} - B : {color.b}");
    }

    void OnColorPickerTransition()
    {
        if (canTransition)
        {
            if(show)
            {
                float alpha = canvasGroup.alpha;

                if(alpha < 1.0f)
                {
                    alpha = Mathf.Lerp(alpha, 1.0f, transitionSpeed * Time.smoothDeltaTime);

                    canvasGroup.alpha = alpha;

                    if (alpha >= 1.0f)
                    {
                        canvasGroup.alpha = 1.0f;
                        canTransition = false;
                    }
                }
            }
            else
            {
                float alpha = canvasGroup.alpha;

                if (alpha > 0.0f)
                {
                    alpha = Mathf.Lerp(alpha, 0.0f, transitionSpeed * Time.smoothDeltaTime);

                    canvasGroup.alpha = alpha;

                    if (alpha <= 0.0f)
                    {
                        canvasGroup.alpha = 0.0f;
                        canTransition = false;
                    }
                }
            }
        }
        else
            return;
    }

    #endregion
}
