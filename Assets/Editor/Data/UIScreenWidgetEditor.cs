using UnityEditor;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    [CustomEditor(typeof(InputActionHandler)), CanEditMultipleObjects]

    public class UIScreenWidgetEditor : Editor
    {
        #region Components

        #region Actions

        public SerializedProperty widgetNameProperty,
                                  inputTypeProperty;

        #endregion

        #region Action Config Properties

        public SerializedProperty buttonComponentConfigProperty,
                                  inputFieldComponentConfigProperty,
                                  inputSliderComponentConfigProperty,
                                  sliderComponentConfigProperty,
                                  checkboxComponentConfigProperty,
                                  dropdownComponentConfigProperty;


        #endregion

        #region Displayer Config Properties

        public SerializedProperty textDisplayerComponentConfigProperty,
                                  imageDisplayerComponentConfigProperty;

        #endregion

        #region Displayers Transitions Properties

        public SerializedProperty imageDisplayerTransitionConfigProperty;

        #endregion

        #endregion

        #region Main

        private void OnEnable()
        {
            #region Components

            widgetNameProperty = serializedObject.FindProperty("name");
            inputTypeProperty = serializedObject.FindProperty("inputType");

            #endregion

            #region Action Config Properties

            buttonComponentConfigProperty = serializedObject.FindProperty("buttonComponentConfig");
            inputFieldComponentConfigProperty = serializedObject.FindProperty("inputFieldComponentConfig");
            inputSliderComponentConfigProperty = serializedObject.FindProperty("inputSliderComponentConfig");
            sliderComponentConfigProperty = serializedObject.FindProperty("sliderComponentConfig");
            checkboxComponentConfigProperty = serializedObject.FindProperty("checkboxComponentConfig");
            dropdownComponentConfigProperty = serializedObject.FindProperty("dropdownComponentConfig");

            #endregion

            #region Displayer Config Properties

            textDisplayerComponentConfigProperty = serializedObject.FindProperty("textComponentConfig");
            imageDisplayerComponentConfigProperty = serializedObject.FindProperty("imageComponentConfig");

            #endregion

            #region Displayer Transitions Properties

            imageDisplayerTransitionConfigProperty = serializedObject.FindProperty("transitionableUIMounts");

            #endregion
        }

        public override void OnInspectorGUI()
        {
            InputActionHandler widget = target as InputActionHandler;

            serializedObject.Update();

            GUILayout.Space(2);
            EditorGUILayout.PropertyField(widgetNameProperty);

            GUILayout.Space(2);
            EditorGUILayout.PropertyField(inputTypeProperty);

            AppData.InputType inputType = (AppData.InputType)inputTypeProperty.enumValueIndex;

            if (inputType != AppData.InputType.None)
            {
                switch (inputType)
                {
                    case AppData.InputType.Button:

                        GUILayout.Space(2);
                        EditorGUILayout.PropertyField(buttonComponentConfigProperty, new GUIContent("Config"));

                        break;

                    case AppData.InputType.InputField:

                        GUILayout.Space(2);
                        EditorGUILayout.PropertyField(inputFieldComponentConfigProperty, new GUIContent("Config"));

                        break;

                    case AppData.InputType.InputSlider:

                        GUILayout.Space(2);
                        EditorGUILayout.PropertyField(inputSliderComponentConfigProperty, new GUIContent("Config"));

                        break;

                    case AppData.InputType.Slider:

                        GUILayout.Space(2);
                        EditorGUILayout.PropertyField(sliderComponentConfigProperty, new GUIContent("Config"));

                        break;

                    case AppData.InputType.DropDown:

                        GUILayout.Space(2);
                        EditorGUILayout.PropertyField(dropdownComponentConfigProperty, new GUIContent("Config"));

                        break;

                    case AppData.InputType.Checkbox:

                        GUILayout.Space(2);
                        EditorGUILayout.PropertyField(checkboxComponentConfigProperty, new GUIContent("Config"));

                        break;


                    case AppData.InputType.Text:

                        GUILayout.Space(2);
                        EditorGUILayout.PropertyField(textDisplayerComponentConfigProperty, new GUIContent("Config"));

                        break;

                    case AppData.InputType.Image:

                        GUILayout.Space(2);
                        EditorGUILayout.PropertyField(imageDisplayerComponentConfigProperty, new GUIContent("Config"));

                        GUILayout.Space(2);
                        EditorGUILayout.PropertyField(imageDisplayerTransitionConfigProperty, new GUIContent("Image Transition Mounts"));

                        break;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}