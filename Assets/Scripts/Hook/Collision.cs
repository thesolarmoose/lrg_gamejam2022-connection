using UnityEngine;

namespace Hook
{
    public class Collision
    {
        private Connection _first;
        private Connection _second;
        private Vector2 _collisionPoint;
        private float _collisionSpeed;

        public Connection First => _first;

        public Connection Second => _second;

        public Vector2 CollisionPoint => _collisionPoint;

        public float CollisionSpeed => _collisionSpeed;

        public Collision(Connection first, Connection second, Vector2 collisionPoint, float collisionSpeed)
        {
            _first = first;
            _second = second;
            _collisionPoint = collisionPoint;
            _collisionSpeed = collisionSpeed;
        }
    }
}