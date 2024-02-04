using UnityEngine;

namespace gishadev.fort.Money
{
    public interface IMoneySpawner
    {
        void BurstSpawnMoney(Vector3 position, int count, float burstForce);
    }
}