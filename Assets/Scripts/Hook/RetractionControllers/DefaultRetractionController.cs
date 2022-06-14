using System;
using System.Collections;
using UnityEngine;
using Utils.Tweening;

namespace Hook.RetractionControllers
{
    [Serializable]
    public class DefaultRetractionController : IRetractionController
    {
        [SerializeField] private float _retractionMaxSpeed;
        [SerializeField] private float _retractionInitialSpeed;
        [SerializeField] private float _retractionAcceleration;
        [SerializeField] private LayerMask _hookablesMask;

        private RaycastHit2D[] _raycastHitsBuffer = new RaycastHit2D[2];
        
        public IEnumerator Retract(Connection first, Connection second)
        {
            float speed = _retractionInitialSpeed;

            var totalDist = Vector2.Distance(first.Tip.position, second.Tip.position);
            var firstDir = (second.Tip.position - first.Tip.position).normalized;
            var secondDir = -firstDir;

            var firstTransform = first.Hooked.GetTransform();
            var secondTransform = second.Hooked.GetTransform();
            var firstCollider = first.Hooked.GetCollider();
            var secondCollider = second.Hooked.GetCollider();

            var filter = new ContactFilter2D {layerMask = _hookablesMask};
            
            firstCollider.Cast(firstDir, filter, _raycastHitsBuffer);
            var firstHit = _raycastHitsBuffer[0];
            var firstDistanceOffset = totalDist - firstHit.distance;
            var firstMoveDistance = totalDist * 0.5f - firstDistanceOffset;
            Vector2 firstInitialPosition = firstTransform.position;
            var firstTargetPosition = firstInitialPosition + firstMoveDistance * firstDir;
            
            
            secondCollider.Cast(secondDir, filter, _raycastHitsBuffer);
            var secondHit = _raycastHitsBuffer[0];
            var secondDistanceOffset = (totalDist - secondHit.distance) * 0.5f;
            var secondMoveDistance = totalDist * 0.5f - secondDistanceOffset;
            Vector2 secondInitialPosition = secondTransform.position;
            var secondTargetPosition = secondInitialPosition + secondMoveDistance * secondDir;

            // hacer cast para ver con que hookable choca
            // si hay un hookable en el medio, ver quién choca primero
            // si no, entonces calcular punto medio

            float firstCurrentDistanceMoved = 0;
            float secondCurrentDistanceMoved = 0;
            
            while (firstCurrentDistanceMoved < firstMoveDistance)
            {
                firstCurrentDistanceMoved = Mathf.Min(firstMoveDistance, firstCurrentDistanceMoved + speed);
                secondCurrentDistanceMoved = Mathf.Min(secondMoveDistance, secondCurrentDistanceMoved + speed);
                speed = Mathf.Min(_retractionMaxSpeed, speed + _retractionAcceleration);

                var firstDeltaMovement = firstDir * firstCurrentDistanceMoved;
                var firstPosition = firstInitialPosition + firstDeltaMovement;
                firstTransform.position = firstPosition;
                
                var secondDeltaMovement = secondDir * secondCurrentDistanceMoved;
                var secondPosition = secondInitialPosition + secondDeltaMovement;
                secondTransform.position = secondPosition;
                
                yield return null;
            }
        }
    }
}