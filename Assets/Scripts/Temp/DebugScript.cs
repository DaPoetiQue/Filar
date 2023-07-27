using TMPro;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class DebugScript : AppMonoBaseClass
    {
        [SerializeField]
        AppData.SceneDataPackets dataPackets;

        TMP_Text debugText = null;

        private void Start()
        {
            debugText = this.GetComponent<TMP_Text>();
        }

        // Update is called once per frame
        public void DebugData()
        {
            debugText.text = "Unity_RG : Button Pressed.";

            Debug.Log("--> Unity_RG : Button Pressed.");

            if (ScreenUIManager.Instance != null)
                ScreenUIManager.Instance.ShowScreenAsync(dataPackets);
            else
                Debug.LogWarning("--> Screen Manager Missing.");
        }
    }
}
