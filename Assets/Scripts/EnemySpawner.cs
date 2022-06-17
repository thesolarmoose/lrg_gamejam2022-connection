using Terrain;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private TerrainReference _terrainReference;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _firstTimeOffset;
        [SerializeField] private float _minTime;
        [SerializeField] private float _maxTime;
        
        [Space, SerializeField] private float _minDistanceToPlayer;
        [SerializeField] private Transform _player;

        private float _nextTimeToSpawn;

        private bool ShouldSpawn => Time.time > _nextTimeToSpawn;

        private void ComputeNextTime()
        {
            float randomCooldown = Random.Range(_minTime, _maxTime);
            _nextTimeToSpawn = Time.time + randomCooldown;
        }

        private Vector2 ComputeSpawnPosition()
        {
            Vector2 position;
            float playerDist;
            int attempts = 5;
            int attempt = 0;
            do
            {
                position = _terrainReference.Terrain.GetRandomPointInside();
                Vector2 playerPos = _player.position;
                playerDist = Vector2.Distance(playerPos, position);
                attempt++;
            } while (playerDist < _minDistanceToPlayer && attempt <= attempts);
            
            return position;
        }

        private void Start()
        {
            ComputeNextTime();
            _nextTimeToSpawn += _firstTimeOffset;
        }

        private void Update()
        {
            if (ShouldSpawn)
            {
                var position = ComputeSpawnPosition();
                Instantiate(_prefab, position, Quaternion.identity, transform);
                
                ComputeNextTime();
            }
        }
    }
}