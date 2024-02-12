﻿using Cysharp.Threading.Tasks;
using gishadev.fort.Shop;
using Sirenix.OdinInspector;
using Unity.AI.Navigation;
using UnityEngine;

namespace gishadev.fort.World
{
    public class Location : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface _navMeshSurface;

        private void OnEnable() => ShopBuyHandler.BuySucceeded += OnBuySucceeded;
        private void OnDisable() => ShopBuyHandler.BuySucceeded -= OnBuySucceeded;

        private void OnBuySucceeded(ShopBuyHandler buyHandler)
        {
            if (buyHandler.Buyable is StructureBuyable)
                UpdateNavmeshSurface();
        }

        [Button]
        private async void UpdateNavmeshSurface()
        {
            await UniTask.NextFrame();
            if (_navMeshSurface != null && _navMeshSurface.isActiveAndEnabled)
                _navMeshSurface.BuildNavMesh();
        }
    }
}