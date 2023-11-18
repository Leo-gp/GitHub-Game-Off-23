using main.entity.Card_Management.Card_Data;
using main.entity.Turn_System;
using main.service.Turn_System;
using Zenject;

namespace main.infrastructure
{
    public class TurnServiceInstaller : Installer<TurnServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<Turn>()
                .FromMethod(CreateTurn)
                .AsSingle();
            
            Container.Bind<TurnService>().AsSingle().NonLazy();
        }
        
        private static Turn CreateTurn()
        {
            var initialTime = new UnitTime { Time = 10 };
            return new Turn(initialTime, 0, initialTime);
        }
    }
}