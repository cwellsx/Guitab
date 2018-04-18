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

        internal IEnumerable<Bar> bars {  get { return listBars; } }

        internal Bar this[int index] {  get { return listBars[index]; } }

        internal int getMsec(int beatsPerMinute)
        {
            return listBars.Sum(bar => bar.getMsec(beatsPerMinute));
        }
    }
}
