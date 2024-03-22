using UnityEngine;

namespace Character
{
    public class GunLooker : BaseLooker
    {
        protected override void _Look(Vector2 dir)
        {
            if (dir == Vector2.zero)
            {
                return;
            }
            var t = transform;
            t.right = dir;
            var scale = t.lossyScale;
            scale.y = dir.x > 0 ? 1 : -1;
            t.localScale = scale;
        }
    }
}