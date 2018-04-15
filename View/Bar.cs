using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Guitab.View
{
    class ViewBar : Canvas
    {
        Line cursor;

        internal ViewBar()
        {

            Width = 220;
            Height = 190;

            for (int i = 0; i < 6; ++i)
            {
                newHorizontal(20, 200, 50 + 20 * i);
            }

            this.Background = Brushes.Ivory;
        }

        void newHorizontal(double x1, double x2, double y)
        {
            newLine(x1, x2, y, y, Brushes.Black);
        }

        Line newLine(double x1, double x2, double y1, double y2, Brush brush)
        {
            Line line = new Line();
            line.X1 = x1;
            line.X2 = x2;
            line.Y1 = y1;
            line.Y2 = y2;

            line.StrokeThickness = 1;
            line.Stroke = brush;
            line.Fill = brush;

            // https://stackoverflow.com/questions/2879033/how-do-you-draw-a-line-on-a-canvas-in-wpf-that-is-1-pixel-thick
            line.SnapsToDevicePixels = true;
            line.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);

            base.Children.Add(line);

            return line;
        }

        internal void TimerTick(long msec)
        {
            if (cursor == null)
            {
                cursor = newLine(20, 20, 50, 150, Brushes.Red);
            }

            //msec -= msec % 125;

            SetLeft(cursor, 180 * msec / 2000);
        }
    }
}
