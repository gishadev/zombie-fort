using Cysharp.Threading.Tasks;
using gishadev.fort.Player;
using gishadev.fort.World.Shop;
using Sirenix.OdinInspector;
using Unity.AI.Navigation;
using UnityEngine;

namespace gishadev.fort.World
{
    public class Location : MonoBehaviour
    {
        [Required] [SerializeField] private NavMeshSurface navMeshSurface;
        [Required] [SerializeField] private Arsenal arsenal;
        [Required] [SerializeField] private PlayerSpawnpoint playerSpawnpoint;

        public Arsenal Arsenal => arsenal;
        public PlayerSpawnpoint PlayerSpawnpoint => playerSpawnpoint;

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
            if (navMeshSurface != null && navMeshSurface.isActiveAndEnabled)
                navMeshSurface.BuildNavMesh();
        }
    }
}