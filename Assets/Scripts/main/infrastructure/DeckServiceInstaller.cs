using main.entity.Card_Management;
using main.service.Card_Management;
using UnityEngine;
using Zenject;

namespace main.infrastructure
{
    public class DeckServiceInstaller : MonoInstaller<DeckServiceInstaller>
    {
        [SerializeField] private bool shuffleOnCreation;
        
        public override void InstallBindings()
        {
            Container.Bind<CardPile>()
                .WithId("DeckService")
                .FromMethod(CreateDeckPile)
                .AsTransient();
            
            Container.BindInterfacesAndSelfTo<DeckService>()
                .FromMethod(CreateDeckService)
                .AsSingle()
                .NonLazy();
        }

        private CardPile CreateDeckPile(InjectContext ctx)
        {
            var starterDeck = ctx.Container.Resolve<StarterDeck>();
            return new CardPile(starterDeck.Cards, shuffleOnCreation);
        }

        private static DeckService CreateDeckService(InjectContext ctx)
        {
            var cardPile = ctx.Container.ResolveId<CardPile>("DeckService");
            return new DeckService(cardPile);
        }
    }
}