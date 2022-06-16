using System.Collections;
using System.Collections.Generic;
using Animations;
using Character;
using UnityEngine;
using Utils;
using Utils.Extensions;
using Utils.Tweening;

namespace DefaultNamespace
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider;
        [SerializeField] private float _speed;
        [SerializeField] private float _timeToDestroy;
        
        [SerializeField] private int _damage;
        [SerializeField] private LayerMask _whoToDamage;
        [SerializeField] private LayerMask _whoToIgnore;

        [Space, SerializeField] private SpritesAnimation _destroyAnimation;

        private RaycastHit2D[] _raycastsBuffer = new RaycastHit2D[5];

        public void Shoot(Vector2 dir)
        {
            StartCoroutine(ShootEnumerator(dir.normalized));
        }

        private IEnumerator ShootEnumerator(Vector2 dir)
        {
            var timeToDestroy = Time.time + _timeToDestroy;
            bool collided = false;
            while (Time.time < timeToDestroy && !collided)
            {
                float moveDist = _speed * Time.deltaTime;
                var filter = new ContactFilter2D
                {
                    useTriggers = true
                };
                int hits = _collider.Cast(dir, filter, _raycastsBuffer, moveDist);
                bool didHit = hits > 0;

                Vector2 movePosition;
                if (didHit && FindProperHit(hits, out var hit))
                {
                    var obj = hit.collider.gameObject;
                    DoDamage(obj);
                    DestroyBullet();
                    movePosition = (Vector2) transform.position + hit.centroid;
                    collided = true;
                }
                else
                {
                    movePosition = (Vector2) transform.position + moveDist * dir;
                }

                transform.position = movePosition;

                yield return null;
            }
            
            DestroyBullet();
        }

        private void DestroyBullet()
        {
            var coroutine = CoroutineUtils.CoroutineSequence(new List<IEnumerator>
            {
                CoroutineUtils.ActionCoroutine(() => _collider.enabled = false),
                _destroyAnimation.Play(Curves.Linear),
                CoroutineUtils.ActionCoroutine(() => Destroy(gameObject)),
            });
            StartCoroutine(coroutine);
        }

        private void DoDamage(GameObject obj)
        {
            bool doDamage = _whoToDamage.IsLayerInMask(obj.layer);
            if (doDamage)
            {
                var health = obj.GetComponent<Health>();
                health.Damage(_damage);
            }
        }

        private bool FindProperHit(int hitsCount, out RaycastHit2D hit)
        {
            for (int i = 0; i < hitsCount; i++)
            {
                var rayCast = _raycastsBuffer[i];
                bool ignore = _whoToIgnore.IsLayerInMask(rayCast.collider.gameObject.layer);
                if (!ignore)
                {
                    hit = rayCast;
                    return true;
                }
            }

            hit = default;
            return false;
        }
    }
}