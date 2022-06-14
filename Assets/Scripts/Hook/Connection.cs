using UnityEngine;

namespace Hook
{
    public class Connection
    {
        private RopeTip _tip;
        private Hookable _hooked;

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