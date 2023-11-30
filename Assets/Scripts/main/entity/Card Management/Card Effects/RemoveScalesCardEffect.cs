using main.entity.Card_Management.Card_Data;
using main.service.Fish_Management;
using UnityEngine;
using Zenject;

namespace main.entity.Card_Management.Card_Effects
{
    [CreateAssetMenu(fileName = "Remove Scales", menuName = "Data/Card Effect/Remove Scales")]
    public class RemoveScalesCardEffect : CardEffect, IPreviewable
    {
        [SerializeField] private int amountOfScalesToRemove;

        private int currentAmountOfScalesToRemove;

        private FishService fishService;

        [Inject]
        public void Construct(FishService fishService)
        {
            this.fishService = fishService;
        }

        private void OnEnable()
        {
            ResetEffect();
        }

        public override void Execute()
        {
            fishService.ScaleFish(currentAmountOfScalesToRemove);
            ResetEffect();
        }

        public override void MultiplyEffect(int multiplier)
        {
            currentAmountOfScalesToRemove *= multiplier;
        }

        protected override void ResetEffect()
        {
            currentAmountOfScalesToRemove = amountOfScalesToRemove;
        }
        
        public int PreviewAmount()
        {
            return currentAmountOfScalesToRemove;
        }
    }
}