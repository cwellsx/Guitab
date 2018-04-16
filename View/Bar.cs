using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

using Guitab.Model.Glyphs;

namespace Guitab.View
{
    class Bar : Canvas
    {
        Line cursor;

        internal Bar(Model.Bar modelBar)
        {
            Width = 220;
            Height = 190;

            for (int i = 0; i < 6; ++i)
            {
                newHorizontal(20, 200, lineY(i));
            }

            foreach (Chord chord in modelBar.chords)
            {
                newLabel(
                    lineY(6),
                    timeX(chord.time, modelBar.beatsPerBar),
                    chord.name,
                    false
                    );
            }

            foreach (Note note in modelBar.notes)
            {
                newLabel(
                    lineY(note.line),
                    timeX(note.time, modelBar.beatsPerBar),
                    note.fret.ToString(),
                    true
                    );
            }

            this.Background = backgroundBrush;
        }

        static Brush backgroundBrush = Brushes.Ivory;

        static int lineY(int line)
        {
            return 150 - (20 * line);
        }

        static double timeX(double time, int beatsPerBar)
        {
            return (180 * (time / beatsPerBar)) + 20;
        }

        void newLabel(double y, double x, string text, bool opaqueBackground)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;

            // https://stackoverflow.com/questions/9264398/how-to-calculate-wpf-textblock-width-for-its-known-font-size-and-characters
            textBlock.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            textBlock.Arrange(new Rect(textBlock.DesiredSize));
            double width = textBlock.ActualWidth;
            double height = textBlock.ActualHeight;

            if (opaqueBackground)
                textBlock.Background = backgroundBrush;

            Canvas.SetTop(textBlock, y - (height / 2));
            Canvas.SetLeft(textBlock, x);

            base.Children.Add(textBlock);
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
