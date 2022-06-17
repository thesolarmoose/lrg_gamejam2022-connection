using System;
using UnityEngine;
using Utils;
using Utils.Extensions;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _firstTimeOffset;
        [SerializeField] private float _minTime;
        [SerializeField] private float _maxTime;
        [SerializeField, DrawRectInScene] private Rect _spawnSpace;
        
        [Space, SerializeField] private float _minDistanceToPlayer;
        [SerializeField] private Transform _player;

        private float _nextTimeToSpawn;

        private bool ShouldSpawn => Time.time > _nextTimeToSpawn;

        public Rect SpawnSpace => _spawnSpace;

        private void ComputeNextTime()
        {
            float randomCooldown = Random.Range(_minTime, _maxTime);
            _nextTimeToSpawn = Time.time + randomCooldown;
        }

        private Vector2 ComputeSpawnPosition()
        {
            var position = _spawnSpace.RandomPositionInside();
            Vector2 playerPos = _player.position;
            var playerDist = Vector2.Distance(playerPos, position);
            if (playerDist < _minDistanceToPlayer)
            {
                var dirToCenter = _spawnSpace.center - playerPos;
                dirToCenter.Normalize();
                position = dirToCenter * _minDistanceToPlayer;
            }

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