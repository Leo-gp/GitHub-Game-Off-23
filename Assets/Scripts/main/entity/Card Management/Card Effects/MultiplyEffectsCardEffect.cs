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
                .ForEach(card => card.MultiplyEffects(multiplier));
            
            ResetEffect();
        }

        public override void MultiplyEffect(int multiplier)
        {
            this.multiplier *= multiplier;
        }

        protected override void ResetEffect()
        {
            multiplier = 2;
        }
    }
}