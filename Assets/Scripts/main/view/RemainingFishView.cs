using main.service.Fish_Management;
using main.service.Turn_System;
using TMPro;
using UnityEngine;
using Zenject;

namespace main.view
{
    public class RemainingFishView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _remainingFishText;

        private FishService fishService;
        private GameService gameService;

        private void OnEnable()
        {
            fishService.OnFishHasBeenScaled.AddListener(RenderRemainingFish);
            Render(0);
        }

        private void OnDisable()
        {
            fishService.OnFishHasBeenScaled.RemoveListener(RenderRemainingFish);
        }

        [Inject]
        public void Construct(GameService gameService, FishService fishService)
        {
            this.gameService = gameService;
            this.fishService = fishService;
        }

        private void Render(int remainingFish)
        {
            _remainingFishText.text = remainingFish + "/" + gameService.RequiredAmountOfFishToScaleToWin();
        }

        private void RenderRemainingFish()
        {
            Render(gameService.CurrentAmountOfScaledFish());
        }
    }
}