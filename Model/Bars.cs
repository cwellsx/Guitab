using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guitab.Model
{
    class Bars
    {
        List<Bar> listBars;

        internal Bars(IEnumerable<Bar> bars)
        {
            listBars = new List<Bar>(bars);
        }

        internal IEnumerable<Bar> bars { get { return listBars; } }

        internal Bar this[int index] { get { return listBars[index]; } }

        internal int nBars { get { return listBars.Count; } }

        internal int getMsec(int beatsPerMinute)
        {
            return listBars.Sum(bar => bar.getMsec(beatsPerMinute));
        }

        internal When? getWhen(long msec, int beatsPerMinute, int sliderBarOffset)
        {
            if (sliderBarOffset < 1)
                throw new Exception();

            for (int i = sliderBarOffset - 1; i < listBars.Count; ++i)
            {
                int msecBarDuration = listBars[i].getMsec(beatsPerMinute);
                if (msecBarDuration > msec)
                {
                    // found it
                    return new When(i, (int)msec, msecBarDuration);
                }
                msec -= msecBarDuration;
            }

            return null;
        }
    }
}
