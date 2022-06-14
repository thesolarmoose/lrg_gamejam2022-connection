using UnityEngine;

namespace Hook
{
    public class Connection
    {
        private RopeTip _tip;
        private IHookable _hooked;

        public RopeTip Tip => _tip;
        public IHookable Hooked => _hooked;
        

        public Connection(RopeTip tip, IHookable hooked)
        {
            _tip = tip;
            _hooked = hooked;
        }

        public void Deconstruct(out RopeTip tip, out IHookable hooked)
        {
            tip = _tip;
            hooked = _hooked;
        }
    }
}