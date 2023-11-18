using main.entity.Card_Management;
using main.service.Card_Management;
using Zenject;

namespace main.infrastructure
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            ResourceLoaderInstaller.Install(Container);
            
            DeckDefinitionRepositoryInstaller.Install(Container);
            
            StarterDeckServiceInstaller.Install(Container);
            
            CardPoolServiceInstaller.Install(Container);

            Container.Bind<PlayerHand>().AsSingle();
            
            Container.Bind<DeckService>().AsSingle();
            
            Container.Bind<DiscardPileService>().AsSingle();
            
            Container.Bind<PlayerHandService>().AsSingle().NonLazy();
            
            TurnServiceInstaller.Install(Container);
        }
    }
}