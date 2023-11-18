using main.entity.Card_Management;
using main.service.Card_Management;
using Zenject;

namespace main.infrastructure
{
    public class PlayerHandServiceInstaller : MonoInstaller<PlayerHandServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerHand>().AsSingle();
            
            Container.Bind<DeckService>().AsSingle();
            
            Container.Bind<DiscardPileService>().AsSingle();
            
            Container.Bind<PlayerHandService>().AsSingle().NonLazy();
        }
    }
}