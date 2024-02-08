using gishadev.fort.Enemy;
using gishadev.fort.Money;
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
        }
    }
}