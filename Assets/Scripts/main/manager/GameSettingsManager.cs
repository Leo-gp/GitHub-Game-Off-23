using System;
using UnityEngine;

namespace Core
{
    public static class GameSettingsManager
    {
        public static bool muteMusic;
        public static bool muteSounds;
        public static bool enableCameraFollow;
        public static bool skipScaleAnimations;

        public static Action onSettingsChanged;

        public static void Load()
        {
            if (HasBeenSetOnce())
            {
                muteMusic = PlayerPrefs.GetInt("muteMusic") == 1;
                muteSounds = PlayerPrefs.GetInt("muteSounds") == 1;
                enableCameraFollow = PlayerPrefs.GetInt("enableCameraFollow") == 1;
                skipScaleAnimations = PlayerPrefs.GetInt("skipScaleAnimations") == 1;
            }
            else
            {
                LoadDefaultValues();
            }
        }

        public static void Save()
        {
            PlayerPrefs.SetInt("muteMusic", muteMusic ? 1 : 0);
            PlayerPrefs.SetInt("muteSounds", muteSounds ? 1 : 0);
            PlayerPrefs.SetInt("enableCameraFollow", enableCameraFollow ? 1 : 0);
            PlayerPrefs.SetInt("skipScaleAnimations", skipScaleAnimations ? 1 : 0);
            AudioManager.UpdateBus();
            onSettingsChanged?.Invoke();
        }

        public static void LoadDefaultValues()
        {
            muteMusic = false;
            muteSounds = false;
            enableCameraFollow = true;
            skipScaleAnimations = false;
        }

        private static bool HasBeenSetOnce()
        {
            return PlayerPrefs.HasKey("muteMusic");
        }
    }
}