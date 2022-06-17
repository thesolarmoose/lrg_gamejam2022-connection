using System;
using System.Collections;
using UnityEngine;

namespace Hook
{
    [Serializable]
    public class RetractionController
    {
        [SerializeField] private float _retractionMaxSpeed;
        [SerializeField] private float _retractionInitialSpeed;
        [SerializeField] private float _retractionAcceleration;
        [SerializeField] private LayerMask _hookablesMask;

        private RaycastHit2D[] _raycastHitsBuffer = new RaycastHit2D[20];
        
        public IEnumerator Retract(Connection first, Connection second)
        {
            var (firstTip, firstHooked) = first;
            var (secondTip, secondHooked) = second;
            var firstDir = (secondTip.position - firstTip.position).normalized;
            var secondDir = -firstDir;

            var firstTransform = firstHooked.Transform;
            var secondTransform = secondHooked.Transform;
            var firstCollider = firstHooked.Collider;
            var secondCollider = secondHooked.Collider;
            bool firstCanMove = firstHooked.IsMovable;
            bool secondCanMove = secondHooked.IsMovable;

            bool anyCanMove = firstCanMove || secondCanMove;
            
            if (anyCanMove)
            {
                float speed = _retractionInitialSpeed;
                bool bothCanMove = firstCanMove && secondCanMove;
                Vector2 firstInitialPosition = firstTransform.position;
                Vector2 secondInitialPosition = secondTransform.position;
                var moveDistance = ComputeMoveDistance(firstCollider, secondCollider, firstDir, bothCanMove);

                float distanceMoved = 0;
            
                while (distanceMoved < moveDistance)
                {
                    distanceMoved = Mathf.Min(moveDistance + Mathf.Epsilon, distanceMoved + speed);
                    speed = Mathf.Min(_retractionMaxSpeed, speed + _retractionAcceleration);

                    if (firstCanMove)
                    {
                        var firstDeltaMovement = firstDir * distanceMoved;
                        var firstPosition = firstInitialPosition + firstDeltaMovement;
                        firstTransform.position = firstPosition;
                    }

                    if (secondCanMove)
                    {
                        var secondDeltaMovement = secondDir * distanceMoved;
                        var secondPosition = secondInitialPosition + secondDeltaMovement;
                        secondTransform.position = secondPosition;
                    }
                
                    yield return null;
                }
            }

            bool firstExists = firstHooked != null && firstHooked.gameObject.activeInHierarchy;
            bool secondExists = secondHooked != null && secondHooked.gameObject.activeInHierarchy;
            bool bothExist = firstExists && secondExists;

            if (bothExist)
            {
                firstHooked.OnCollided(secondHooked);
                secondHooked.OnCollided(firstHooked);
            }
        }

        private float ComputeMoveDistance(
            Collider2D firstCollider,
            Collider2D secondCollider,
            Vector2 dir,
            bool bothMove)
        {
            var filter = new ContactFilter2D
            {
                layerMask = _hookablesMask,
                useTriggers = true,
            };
            int hits = firstCollider.Cast(dir, filter, _raycastHitsBuffer);
            if (FindColliderInRayCastList(hits, secondCollider, out var hit))
            {
                var distance = hit.distance;
                float moveDistance = bothMove ? distance * 0.5f : distance;
                return moveDistance;
            }

            Debug.LogWarning("did not find other collider");
            return 0;
        }

        private bool FindColliderInRayCastList(int hitsCount, Collider2D collider, out RaycastHit2D raycast)
        {
            for (int i = 0; i < hitsCount; i++)
            {
                var ray = _raycastHitsBuffer[i];
                if (ray.collider == collider)
                {
                    raycast = ray;
                    return true;
                }
            }

            raycast = default;
            return false;
        }
    }
}