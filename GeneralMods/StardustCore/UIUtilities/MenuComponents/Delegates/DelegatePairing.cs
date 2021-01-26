using System.Collections.Generic;

namespace StardustCore.UIUtilities.MenuComponents.Delegates
{
    public class DelegatePairing
    {
        public Delegates.paramaterDelegate click;
        public List<object> paramaters;

        public DelegatePairing(Delegates.paramaterDelegate buttonDelegate, List<object> Paramaters)
        {
            this.click = buttonDelegate;
            this.paramaters = Paramaters ?? new List<object>();
        }

        public void run()
        {
            this.click?.Invoke(this.paramaters);
        }
    }
}
