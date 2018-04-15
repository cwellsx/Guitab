using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Guitab.Model.Glyphs;

namespace Guitab.Model
{
    static class InputFile
    {
        internal static Bars Load()
        {
            return new Bars(loadBars());
        }

        static IEnumerable<Bar> loadBars()
        {
            IEnumerable<Glyph> glyphs = loadGlyphs();

            State state = new Model.State();

            int barNumber = 0;
            Bar bar = null;

            foreach (Glyph glyph in glyphs)
            {
                //bool isState = state.Add(note);
                // add to the persistent state
                glyph.setState(state);

                if (glyph.newBarNumber.HasValue)
                {
                    int newNumber = glyph.newBarNumber.Value;

                    // new Bar
                    assert(newNumber == ++barNumber);

                    if (bar != null)
                        yield return bar;
                    bar = new Bar(barNumber, state.Clone());
                    continue;
                }
                if (bar != null)
                {
                    glyph.setBar(bar);
                }
            }
        }

        static IEnumerable<Glyph> loadGlyphs()
        {
            Signature signature = new Signature(4, 4);
            yield return signature;

            for (int i = 1; i < 7; ++i)
            {
                yield return new BarNumber(i);
            }
        }

        static void assert(bool b)
        {
            if (!b)
                throw new Exception();
        }
    }
}
