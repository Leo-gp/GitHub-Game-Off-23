namespace Core
{
    using FMODUnity;
    using UnityEngine;
    using UnityEngine.Assertions;

    public class AudioManager : MonoBehaviour
    {

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public static void UpdateBus()
        {
            var bus = RuntimeManager.GetBus("bus:/");
            if (GameSettingsManager.muteAudio)
            {
                bus.setVolume(0f);
            }
            else
            {
                Assert.IsTrue(GameSettingsManager.volumeStep >= 1 &&
                    GameSettingsManager.volumeStep <= 10,
                    GameSettingsManager.volumeStep.ToString());

                bus.setVolume(0.1f * GameSettingsManager.volumeStep);
            }
        }

    }
}
