using System.Collections.Generic;
using UnityEngine;

namespace Com.RedicalGames.Filar
{
    public class AudioHandler : MonoBehaviour
    {
        #region Components

        [SerializeField]
        AudioSource audioPlayer = null;

        [Space(5)]
        [SerializeField]
        List<AppData.AudioPlayerData> audioPlayersList = new List<AppData.AudioPlayerData>();

        #endregion

        #region Unity Callbacks

        void OnEnable() => OnRegisterToActionEvents(true);

        void OnDisable() => OnRegisterToActionEvents(false);

        void Start() => Init();

        #endregion

        #region Components

        void OnRegisterToActionEvents(bool register)
        {
            if (register)
                AppData.ActionEvents._OnPlayAudioEvent += PlayAudio;
            else
                AppData.ActionEvents._OnPlayAudioEvent -= PlayAudio;
        }

        void Init()
        {
            if (audioPlayersList.Count > 0)
                foreach (var audioPlayer in audioPlayersList)
                    if (audioPlayer.value != null)
                        audioPlayer.Initialize(this.audioPlayer);
                    else
                        Debug.LogWarning($"--> Init Failed : Audio Player Of Type {audioPlayer.audioType}'s Value Missing / Not Assigned In The Editor.");
        }

        void PlayAudio(AppData.AudioType audioType)
        {
            AppData.AudioPlayerData audioPlayer = audioPlayersList.Find((player) => player.audioType == audioType);

            if (audioPlayer != null)
                audioPlayer.PlayAudio();
            else
                Debug.LogWarning($"--> PlayAudio Failed : Audio Player Of Type : {audioType} Missing / Not Found In audioPlayersList");
        }

        #endregion
    }
}
