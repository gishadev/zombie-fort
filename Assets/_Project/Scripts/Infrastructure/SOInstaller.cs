using gishadev.fort.Core;
using gishadev.fort.World.Shop;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SOInstaller", menuName = "Installers/SOInstaller")]
public class SOInstaller : ScriptableObjectInstaller<SOInstaller>
{
    [SerializeField] private GameDataSO gameDataSO;
    [SerializeField] private ShopDataSO shopDataSO;

    public override void InstallBindings()
    {
        Container.BindInstances(gameDataSO, shopDataSO);
    }
}