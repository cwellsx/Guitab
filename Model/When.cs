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
        internal readonly double time;

        internal When(
            int barIndex,
            int msecWithinBar,
            int msecBarDuration,
            State state
            )
        {
            this.barIndex = barIndex;
            this.msecWithinBar = msecWithinBar;
            this.msecBarDuration = msecBarDuration;

            double fraction = (double)msecWithinBar / msecBarDuration;
            int interval = (int)(fraction * state.nIntervalsPerBar);
            this.time = (double)interval / state.nIntervalsPerBeat;
        }
    }
}
