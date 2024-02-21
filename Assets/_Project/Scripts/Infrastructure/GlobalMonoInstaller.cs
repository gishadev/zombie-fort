using gishadev.fort.Enemy;
using gishadev.fort.Money;
using gishadev.fort.Player;
using Zenject;

namespace gishadev.fort.Infrastructure
{
    public class GlobalMonoInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.Bind<IMoneyController>().To<MoneyController>().AsSingle().NonLazy();
            Container.Bind<IMoneySpawner>().To<MoneySpawner>().AsSingle().NonLazy();
            Container.Bind<IEnemySpawner>().To<EnemySpawner>().AsSingle().NonLazy();
            Container.Bind<IPlayerInventoryController>().To<PlayerInventoryController>().AsSingle().NonLazy();
        }
    }
}