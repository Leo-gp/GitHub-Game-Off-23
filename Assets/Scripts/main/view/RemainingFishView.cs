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
        
        [Inject]
        public void Construct(FishService fishService)
        {
            this.fishService = fishService;
        }

        [Inject]
        public void Construct(GameService gameService)
        {
            this.gameService = gameService;
        }

        private void Start()
        {
            fishService.OnFishHasBeenScaled.AddListener(RenderRemainingFish);
            Render(0);
        }

        private void Render(int remainingFish)
        {
            _remainingFishText.text = remainingFish.ToString() + "/" + gameService.RequiredAmountOfFishToScaleToWin().ToString();
        }

        private void RenderRemainingFish(){
            Render(gameService.CurrentAmountOfScaledFish());
        }
    }
}