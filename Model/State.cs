using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Guitab.Model.Glyphs;

namespace Guitab.Model
{
    class State
    {
        internal Tempo tempo;
        internal Signature signature;

        internal int beatsPerMinute { get { return tempo.bpm; } }
        internal int beatsPerBar { get { return signature.beatsPerBar; } }
        internal int beatUnit { get { return signature.beatUnit; } }

        internal State Clone()
        {
            return (State)this.MemberwiseClone();
        }
    }
}
