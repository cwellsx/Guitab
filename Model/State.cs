﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Guitab.Model.Glyphs;

namespace Guitab.Model
{
    class State
    {
        internal Tempo tempo { set; private get; }
        internal Signature signature { set; private get; }

        int beatsPerMinute { get { return tempo.bpm; } }
        internal int beatsPerBar { get { return signature.beatsPerBar; } }
        internal int beatUnit { get { return signature.beatUnit; } }
        internal int nIntervalsPerBar { get { return signature.nIntervalsPerBar; } }
        internal int nIntervalsPerBeat { get { return signature.nIntervalsPerBeat; } }

        internal State Clone()
        {
            return (State)this.MemberwiseClone();
        }
    }
}
