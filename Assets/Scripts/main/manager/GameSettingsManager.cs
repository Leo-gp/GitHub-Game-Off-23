namespace Core
{
    using UnityEngine;

    public static class GameSettingsManager
    {
        public static bool muteAudio;
        public static int volumeStep;

        private const bool DEBUG_MODE = true;

        private static void DebugLog(string message)
        {
            if (DEBUG_MODE) Debug.Log(message);
        }

        public static void Load()
        {
            if (HasBeenSetOnce())
            {
                DebugLog("Has been set up before. Now setting class variables to player prefs");

                muteAudio = PlayerPrefs.GetInt("muteAudio") == 1;
                DebugLog("muteAudio: " + muteAudio);

                volumeStep = PlayerPrefs.GetInt("volumeStep");
                DebugLog("volumeStep: " + volumeStep);
            }
            else LoadDefaultValues();
        }

        public static void Save()
        {
            DebugLog("Saving settings...");

            PlayerPrefs.SetInt("muteAudio", muteAudio ? 1 : 0);
            DebugLog("muteAudio: " + muteAudio);

            PlayerPrefs.SetInt("volumeStep", volumeStep);
            DebugLog("volumeStep: " + volumeStep);

            DebugLog("Now updating master bus...");
            AudioManager.UpdateBus();
        }

        private static void LoadDefaultValues()
        {
            DebugLog("Never been setup, loading default values");

            muteAudio = false;
            volumeStep = 10;
        }

        private static bool HasBeenSetOnce() => PlayerPrefs.HasKey("muteAudio");

    }
}
