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
        internal readonly int barNumber;
        readonly State state;

        internal Bar(int barNumber, State state)
        {
            this.barNumber = barNumber;
            this.state = state;
        }
    }
}
