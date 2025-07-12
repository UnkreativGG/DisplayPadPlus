using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeySimmulator.Macros.MacroActions;
internal class DelayAction(int msToDelay) : IMacroAction
{
    private readonly int MsToDelay = msToDelay;



    public void Execute()
    {
        Thread.Sleep(MsToDelay);
    }
}
