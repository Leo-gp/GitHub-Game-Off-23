using main.entity.Turn_System;
using main.service.Turn_System;
using Zenject;

namespace main.infrastructure
{
    public class GameServiceInstaller : MonoInstaller<GameServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<Game>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameService>().AsSingle().NonLazy();
        }
    }
}