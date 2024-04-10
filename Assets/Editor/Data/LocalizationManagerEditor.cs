using UnityEngine;
using UnityEditor;

namespace Com.RedicalGames.Filar
{
    namespace Com.RedicalGames.Filar
    {
        [CustomEditor(typeof(LocalizationManager))]
        public class LocalizationManagerEditor : Editor
        {
            #region Components

            #endregion

            #region Main

            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                LocalizationManager localizationManager = (LocalizationManager)target;

                GUILayout.Space(15);

                if (GUILayout.Button("Change App Language", GUILayout.Height(50)))
                    localizationManager.ChangeLanguage(localizationManager.GetCurrentLanguage().GetData());
            }

            #endregion
        }
    }
}