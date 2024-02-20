using gishadev.fort.Core;
using UnityEngine;
using Zenject;

namespace gishadev.fort.World.Shop
{
    public class WeaponBuyable : Buyable
    {
        [Inject] private GameDataSO _gameDataSO;
        [Inject] private DiContainer _diContainer;

        protected override void OnBuySuccess()
        {
            var position = transform.position + Vector3.up;
            var parent = transform.root.parent;
            _diContainer.InstantiatePrefab(_gameDataSO.WeaponGiverPrefab, position, Quaternion.identity, parent);
        }
    }
}