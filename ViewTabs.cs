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

namespace Guitab
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Guitab"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Guitab;assembly=Guitab"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ViewTabs/>
    ///
    /// </summary>
    /// 

    // derive from WrapPanel as described at
    // https://docs.microsoft.com/en-us/dotnet/framework/wpf/controls/panels-overview
    public class ViewTabs : WrapPanel
    {
        List<ViewBar> bars;

        internal void LoadTabs(int nBars)
        {
            bars = new List<ViewBar>();

            for(int i = 0; i < nBars; ++i)
            {
                bars.Add(new ViewBar());
            }

            bars.ForEach(bar=>Children.Add(bar));
        }

        internal bool HasLoadedBars { get { return bars != null && bars.Count > 0; } }

        internal void TimerTick(long msec)
        {
            msec = msec % 2000;

            ViewBar bar = bars[0];
            bar.TimerTick(msec);
        }
    }
}
