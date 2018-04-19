using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Guitab.View
{
    // derive from WrapPanel as described at
    // https://docs.microsoft.com/en-us/dotnet/framework/wpf/controls/panels-overview

    // add to XAML as described at
    // https://docs.microsoft.com/en-us/dotnet/framework/wpf/advanced/xaml-namespaces-and-namespace-mapping-for-wpf-xaml

    public class Bars : WrapPanel
    {
        Model.Bars modelBars;
        List<Bar> bars;
        Model.When? previous;

        internal void Load(Model.Bars modelBars)
        {
            Children.Clear();

            this.modelBars = modelBars;

            // create a View.Bar for each Model.Bar
            // and add each new View.Bar to this.Children
            bars = new List<Bar>(modelBars.bars.Select(modelBar =>
            {
                Bar bar = (!modelBar.barNumber.likeOther.HasValue)
                ? new Bar(modelBar)
                : new Bar(modelBar, modelBars[modelBar.barNumber.likeOther.Value - 1]);
                Children.Add(bar);
                return bar;
            }));

            previous = null;
        }

        internal bool HasLoadedBars { get { return bars != null && bars.Count > 0; } }

        internal int getMsec(int beatsPerMinute)
        {
            // delegate to the data
            return modelBars.getMsec(beatsPerMinute);
        }


        // caution: this is called very frequently i.e. as often as possible
        internal bool TimerTick(long msec, int beatsPerMinute, Action<int> setNewBarIndex)
        {
            Model.When? when = modelBars.getWhen(msec, beatsPerMinute);

            if (!when.HasValue)
            {
                // end of play
                StopPlay();
                return false;
            }

            timerTick(when.Value, setNewBarIndex);

            return true; ;
        }

        void timerTick(Model.When when, Action<int> setNewBarIndex)
        {
            if (previous.HasValue)
            {
                if (previous.Value.barIndex != when.barIndex)
                {
                    removePrevious();
                    setNewBarIndex(when.barIndex);
                }
            }

            bars[when.barIndex].TimerTick(when.msecWithinBar, when.msecBarDuration);

            previous = when;
        }

        void removePrevious()
        {
            bars[previous.Value.barIndex].TimerRemove();
            previous = null;
        }

        internal void StopPlay()
        {
            if (previous.HasValue)
            {
                removePrevious();
                previous = null;
            }
        }
    }
}
