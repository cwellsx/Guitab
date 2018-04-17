using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Guitab.Model.Glyphs;

namespace Guitab.Model
{
    class Bar
    {
        internal readonly BarNumber barNumber;
        internal readonly State state;
        internal List<Chord> chords = new List<Chord>();
        internal List<Note> notes = new List<Note>();

        internal Bar(BarNumber barNumber, State state)
        {
            this.barNumber = barNumber;
            this.state = state;
        }

        internal void Add(Chord chord)
        {
            chords.Add(chord);
        }

        internal void Add(Note note)
        {
            notes.Add(note);
        }

        void assert(Glyph glyph)
        {
            if (glyph.time >= state.beatsPerBar)
                throw new Exception();
        }
    }
}
