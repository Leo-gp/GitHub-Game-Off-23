using main.entity.Card_Management.Card_Data;
using main.service.Turn_System;
using UnityEngine;
using Zenject;

namespace main.entity.Card_Management.Card_Effects
{
    [CreateAssetMenu(fileName = "Gain Time", menuName = "Data/Card Effect/Gain Time")]
    public class GainTimeCardEffect : CardEffect
    {
        [SerializeField] private int amount;

        private TurnService turnService;

        [Inject]
        public void Construct(TurnService turnService)
        {
            this.turnService = turnService;
        }
        
        public override void Execute()
        {
            turnService.IncreaseTime(amount);
        }
    }
}