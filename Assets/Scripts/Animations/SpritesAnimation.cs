using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Tweening;

namespace Animations
{
    public class SpritesAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private List<Sprite> _sprites;
        [SerializeField] private float _time;

        public IEnumerator Play(float duration, Curves.TimeCurveFunction curve)
        {
            yield return _renderer.TweenSpritesCoroutine(_sprites, duration, curve);
        }
        
        public IEnumerator Play(Curves.TimeCurveFunction curve)
        {
            yield return Play(_time, curve);
        }
    }
}