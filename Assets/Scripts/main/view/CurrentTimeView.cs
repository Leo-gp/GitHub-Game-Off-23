using main.service.Card_Management;
using TMPro;
using UnityEngine;
using Zenject;

namespace main.view
{
    public class CurrentTimeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentTimeText;

        private PlayerHandService playerHandService;

        private void OnEnable()
        {
            playerHandService.OnTimeUnitChange.AddListener(UpdateCurrentTime);
        }

        private void OnDisable()
        {
            playerHandService.OnTimeUnitChange.RemoveListener(UpdateCurrentTime);
        }

        [Inject]
        public void Construct(PlayerHandService playerHandService)
        {
            this.playerHandService = playerHandService;
        }

        private void UpdateCurrentTime(int currentTime)
        {
            _currentTimeText.text = currentTime.ToString();
        }
    }
}