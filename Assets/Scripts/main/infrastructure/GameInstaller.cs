using main.entity.Card_Management;
using main.entity.Card_Management.Deck_Definition;
using main.repository;
using main.repository.Card_Management.Deck_Definition;
using main.service.Card_Management;
using Zenject;

namespace main.infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LocalizationSettingsWrapper>().AsSingle();
            
            Container.Bind<IResourceLoader<DeckDefinition>>()
                .WithId(ResourcePath.StarterDeck.GetValue())
                .FromMethod(ctx => CreateResourceLoader(ctx, ResourcePath.StarterDeck))
                .AsTransient();

            Container.Bind<IResourceLoader<DeckDefinition>>()
                .WithId(ResourcePath.CardPool.GetValue())
                .FromMethod(ctx => CreateResourceLoader(ctx, ResourcePath.CardPool))
                .AsTransient();
            
            Container.Bind<DeckDefinitionRepository>()
                .WithId(ResourcePath.StarterDeck.GetValue())
                .FromMethod(ctx => CreateDeckDefinitionRepository(ctx, ResourcePath.StarterDeck))
                .AsTransient();

            Container.Bind<DeckDefinitionRepository>()
                .WithId(ResourcePath.CardPool.GetValue())
                .FromMethod(ctx => CreateDeckDefinitionRepository(ctx, ResourcePath.CardPool))
                .AsTransient();
            
            Container.Bind<StarterDeck>().AsSingle();
            
            Container.Bind<StarterDeckService>()
                .FromMethod(CreateStarterDeckService)
                .AsSingle()
                .NonLazy();
            
            Container.Bind<CardPool>().AsSingle();
            
            Container.Bind<CardPoolService>()
                .FromMethod(CreateCardPoolService)
                .AsSingle();
        }
        
        private static ResourceLoader<DeckDefinition> CreateResourceLoader(InjectContext ctx, ResourcePath resourcePath)
        {
            var localizationSettings = ctx.Container.Resolve<LocalizationSettingsWrapper>();
            return new ResourceLoader<DeckDefinition>(localizationSettings, resourcePath);
        }
        
        private static DeckDefinitionRepository CreateDeckDefinitionRepository(InjectContext ctx, ResourcePath resourcePath)
        {
            var loader = ctx.Container.ResolveId<IResourceLoader<DeckDefinition>>(resourcePath.GetValue());
            return new DeckDefinitionRepository(loader);
        }
        
        private static StarterDeckService CreateStarterDeckService(InjectContext ctx)
        {
            var repositoryStarterDeck = ctx.Container.ResolveId<DeckDefinitionRepository>(ResourcePath.StarterDeck.GetValue());
            return new StarterDeckService(ctx.Container.Resolve<StarterDeck>(), repositoryStarterDeck);
        }
        
        private static CardPoolService CreateCardPoolService(InjectContext ctx)
        {
            var repositoryCardPool = ctx.Container.ResolveId<DeckDefinitionRepository>(ResourcePath.CardPool.GetValue());
            return new CardPoolService(ctx.Container.Resolve<CardPool>(), repositoryCardPool, ctx.Container.Resolve<StarterDeck>());
        }
    }
}