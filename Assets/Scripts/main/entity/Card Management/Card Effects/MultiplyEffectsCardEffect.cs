using System;
using System.Linq;
using main.entity.Card_Management.Card_Data;
using UnityEngine;
using Zenject;

namespace main.entity.Card_Management.Card_Effects
{
    [CreateAssetMenu(fileName = "Multiply Effects", menuName = "Data/Card Effect/Multiply Effects")]
    public class MultiplyEffectsCardEffect : CardEffect
    {
        [SerializeField] private CardClass targetClass;

        private int multiplier;

        private int Multiplier
        {
            get => multiplier;
            set
            {
                multiplier = value;
                OnEffectUpdated?.Invoke();
            }
        }

        public override event Action OnEffectUpdated;

        private PlayerHand playerHand;

        [Inject]
        public void Construct(PlayerHand playerHand)
        {
            this.playerHand = playerHand;
        }

        private void OnEnable()
        {
            ResetEffect();
        }

        public override void Execute()
        {
            playerHand.HandCards
                .Where(card => card.CardClass.Equals(targetClass.ToString()))
                .ToList()
                .ForEach(card => card.MultiplyEffects(Multiplier));
            
            ResetEffect();
        }

        public override void MultiplyEffect(int multiplier)
        {
            Multiplier *= multiplier;
        }

        protected override void ResetEffect()
        {
            Multiplier = 2;
        }

        public override string GetDescription()
        {
            return $"x{Multiplier} the effects of all {targetClass.ToString()} cards in your hand";
        }
    }
}