using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using Guitab.Model.Glyphs;

namespace Guitab.Model
{
    static class InputFile
    {
        internal static Bars Load()
        {
            const string path = @"C:\Users\Christopher\Documents\get lucky.txt";
            IEnumerable<string> lines = File.ReadLines(path);
            IEnumerable<Glyph> glyphs = loadGlyphs(lines);
            return new Bars(loadBars(glyphs));
        }

        static IEnumerable<Bar> loadBars(IEnumerable<Glyph> glyphs)
        {
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
            yield return bar;
        }

        // this lets the input file have different formats in different parts of the file
        interface IInputState
        {
            IEnumerable<Glyph> readLine(string line);
            IInputState nextState { get; }
        }

        static class Format
        {
            internal static bool isEmpty(string line)
            {
                if (string.IsNullOrEmpty(line))
                    return true;
                assert(line == line.Trim());
                return false;
            }
            internal static bool isNotes(string line)
            {
                if (isEmpty(line))
                    return false;
                char first = line[0];
                return (first == '.') || char.IsDigit(first);
            }
            internal static IEnumerable<Note> readNotes(string line, double time)
            {
                assert(isNotes(line));
                string[] split = line.Split(' ');
                assert(split.Length == 6);
                return split.Select((s, i) =>
                {
                    if (s == ".")
                        return null;
                    int fret = int.Parse(s);
                    return new Note(i, fret, time);
                }).Where(note => (note != null));
            }
            internal static bool isNewBar(string line)
            {
                return !isEmpty(line) && (line[0] == '#');
            }
            internal static BarNumber readNewBar(string line)
            {
                assert(isNewBar(line));
                string[] split = line.Split(' ');
                int number = int.Parse(split[0].Substring(1));
                if (split.Length == 1)
                {
                    return new BarNumber(number);
                }
                if (split[1] == "like")
                {
                    assert(split[2][0] == '#');
                    return new BarNumber(number, int.Parse(split[2].Substring(1)));
                }
                assert(split[1].StartsWith("(") && split.Last().EndsWith(")"));
                string comment = string.Join(" ", split.Skip(1));
                return new BarNumber(number, comment);
            }
        }

        class StateStart : IInputState
        {
            int nIntervalsPerBar;

            IInputState newState;

            public IInputState nextState
            {
                get
                {
                    return (newState != null) ? newState : this;
                }
            }

            public IEnumerable<Glyph> readLine(string line)
            {
                if (Format.isNotes(line))
                {
                    assert(!Format.readNotes(line, 0).Any());
                    ++nIntervalsPerBar;
                    yield break;
                }

                Signature signature= new Signature(4, 4, nIntervalsPerBar);
                yield return signature;

                assert(nIntervalsPerBar == (signature.beatsPerBar * 2));
                newState = new StateMain(signature, nIntervalsPerBar);
            }
        }

        class StateMain : IInputState
        {
            readonly int nIntervalsPerBar;
            readonly int nIntervalsPerBeat;
            int nIntervals;

            internal StateMain(Signature signature, int nIntervalsPerBar)
            {
                this.nIntervalsPerBar = nIntervalsPerBar;
                this.nIntervalsPerBeat = nIntervalsPerBar / signature.beatsPerBar;
                assert(0 == (nIntervalsPerBar % signature.beatsPerBar));
                // pretend we just finished a bar
                nIntervals = nIntervalsPerBar;
            }

            public IInputState nextState
            {
                get
                {
                    return this;
                }
            }

            public IEnumerable<Glyph> readLine(string line)
            {
                // empty line
                if (Format.isEmpty(line))
                {
                    assert((nIntervals == nIntervalsPerBar) || (nIntervals == 0));
                    yield break;
                }

                // start of bar
                if (nIntervals == nIntervalsPerBar)
                {
                    // end of previous bar so expect new bar now
                    assert(Format.isNewBar(line));
                    BarNumber barNumber = Format.readNewBar(line);
                    if (!barNumber.likeOther.HasValue)
                        nIntervals = 0;
                    // else no content in this bar
                    yield return barNumber;
                    yield break;
                }
                else
                {
                    assert(!Format.isNewBar(line));
                }

                double time = (double)nIntervals / nIntervalsPerBeat;

                // notes
                if (Format.isNotes(line))
                {
                    foreach (Note note in Format.readNotes(line, time))
                        yield return note;
                    ++nIntervals;
                    assert(nIntervals <= nIntervalsPerBar);
                    yield break;
                }

                // else chord
                yield return new Chord(line, time);
                assert(nIntervals < nIntervalsPerBar);
            }
        }

        static IEnumerable<Glyph> loadGlyphs(IEnumerable<string> lines)
        {
            IInputState state = new StateStart();
            int lineNumber = 0;
            foreach (string line in lines)
            {
                ++lineNumber;
                foreach (Glyph glyph in state.readLine(line))
                    yield return glyph;
                state = state.nextState;
            }
        }

        static void assert(bool b)
        {
            if (!b)
                throw new Exception();
        }
    }
}
