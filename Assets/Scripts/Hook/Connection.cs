using System;
using UnityEngine;

namespace Hook
{
    [Serializable]
    public class Connection
    {
        [SerializeField] private RopeTip _tip;
        [SerializeField] private Hookable _hooked;

        public RopeTip Tip => _tip;
        public Hookable Hooked => _hooked;
        

        public Connection(RopeTip tip, Hookable hooked)
        {
            _tip = tip;
            _hooked = hooked;
        }

        public void Deconstruct(out RopeTip tip, out Hookable hooked)
        {
            tip = _tip;
            hooked = _hooked;
        }
    }
}