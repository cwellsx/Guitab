using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            for (int i = 0; i < 10; ++i)
            {
                yield return new Bar();
            }
        }
    }
}
