using System;
using gishadev.fort.Enemy;
using UnityEngine;
using Zenject;

namespace gishadev.fort.Core
{
    public class GameManager : MonoBehaviour
    {
        [Inject] private IEnemySpawner _enemySpawner;
        
        private void Start()
        {
            _enemySpawner.StartSpawning();
        }
    }
}