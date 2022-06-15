using UnityEngine;

namespace Character
{
    public class GunLooker : BaseLooker
    {
        protected override void _Look(Vector2 dir)
        {
            var t = transform;
            t.right = t.lossyScale.x > 0 ? dir : -dir;
        }
    }
}