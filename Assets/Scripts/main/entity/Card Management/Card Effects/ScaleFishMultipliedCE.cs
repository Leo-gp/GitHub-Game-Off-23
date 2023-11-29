using main.entity.Card_Management.Card_Data;
using main.service.Card_Management;
using main.service.Fish_Management;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace main.entity.Card_Management.Card_Effects
{
    [CreateAssetMenu(fileName = "Scale Fish Multiplied", menuName = "Data/Effects/Scale Fish Multiplied")]
    public class ScaleFishMultipliedCE : CardEffect
    {
        [SerializeField] private int _baseAmountOfScales;
        [SerializeField] private int _incrementAmountOfScales;
        [SerializeField] private bool _multiplyOnceForEachCardNamePlayed;
        [SerializeField] public Card playedCardRef;
        [SerializeField] private int _multiplicationLimit;
        private FishService fishService;
        private PlayerHandService playerHandService;
        
        [Inject]
        public void Construct(FishService fishService)
        {
            this.fishService = fishService;
        }

        [Inject]
        public void Construct(PlayerHandService playerHandService)
        {
            this.playerHandService = playerHandService;
        }

        public override void Execute()
        {
            
        }
    }
}