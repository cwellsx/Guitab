using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guitab.Model.Glyphs
{
    abstract class Glyph
    {
        protected Glyph(double time)
        {
            this.time = time;
        }

        internal readonly double time; // time within bar, measured in number of beats

        internal abstract void setState(State state);
        internal void setBar(Bar bar) { setBar(bar, time); }
        internal virtual int? newBarNumber { get { return null; } }
        protected abstract void setBar(Bar bar, double time);
    }

    internal class Tempo : Glyph
    {
        internal readonly int bpm;
        internal Tempo(int bpm, double time = 0) : base(time) { this.bpm = bpm; }
        internal override void setState(State state) { state.tempo = this; }
        protected override void setBar(Bar bar, double time) { throw new NotImplementedException(); }
    }

    internal class Signature : Glyph
    {
        internal readonly int beatsPerBar;
        internal readonly int beatUnit;
        internal Signature(int beatsPerBar, int beatUnit, double time = 0) : base(time) { this.beatsPerBar = beatsPerBar; this.beatUnit = beatUnit; }
        internal override void setState(State state) { state.signature = this; }
        protected override void setBar(Bar bar, double time) { throw new NotImplementedException(); }
    }

    internal class BarNumber : Glyph
    {
        readonly int number;
        internal BarNumber(int number) : base(0) { this.number = number; }
        internal override void setState(State state) { }
        internal override int? newBarNumber { get { return number; } }
        protected override void setBar(Bar bar, double time) { }
    }
}
