using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class AssetImportScreenView : MonoBehaviour
    {
        [SerializeField]
        List<AppData.AssetImportField> importField = new List<AppData.AssetImportField>();

        bool initialized;

        public void Init()
        {
            initialized = true;

            if (initialized)
                OnResetAllStates();
        }

        public void SetImportFieldState(AppData.AssetFieldType fieldType, string arg = null, bool state = false)
        {
            if (!initialized)
                return;

            for (int i = 0; i < importField.Count; i++)
            {
                if (importField[i].assetType == fieldType)
                {
                    importField[i].fieldText.text = arg;
                    importField[i].fieldAssignedIcon.SetActive(state);
                }
            }
        }

        public void OnResetAllStates()
        {
            if (!initialized)
                return;

            for (int i = 0; i < importField.Count; i++)
            {
                importField[i].fieldText.text = importField[i].placeHolderMessage;
                importField[i].fieldAssignedIcon.SetActive(false);
            }
        }
    }
}
