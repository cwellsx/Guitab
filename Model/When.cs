using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guitab.Model
{
    struct When
    {
        internal readonly int barIndex;
        internal readonly int msecWithinBar;
        internal readonly int msecBarDuration;

        internal When(
            int barIndex,
            int msecWithinBar,
            int msecBarDuration
            )
        {
            this.barIndex = barIndex;
            this.msecWithinBar = msecWithinBar;
            this.msecBarDuration = msecBarDuration;
        }
    }
}
