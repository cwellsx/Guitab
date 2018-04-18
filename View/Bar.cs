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
        readonly Model.State state;
        Line cursor;

        enum LabelType { Chord, Note, BarNumber, Comment };

        internal Bar(Model.Bar modelBar, Model.Bar likeOther = null)
        {
            state = modelBar.state;

            Width = 220;
            Height = 190;

            for (int i = 0; i < 6; ++i)
            {
                newHorizontal(20, 200, lineY(i));
            }

            newLabel(
                lineY(5),
                timeX(0, LabelType.BarNumber),
                modelBar.barNumber.number.ToString(),
                LabelType.BarNumber
                );

            string comment = modelBar.barNumber.comment;
            if (string.IsNullOrEmpty(comment) && (likeOther != null))
                comment = string.Format("(like {0})", likeOther.barNumber.number);

            if (!string.IsNullOrEmpty(comment))
            {
                newLabel(
                    lineY(-1),
                    timeX(0, LabelType.Comment),
                    comment,
                    LabelType.Comment
                    );
            }

            if (likeOther != null)
                showContents(likeOther);
            else
                showContents(modelBar);

            this.Background = backgroundBrush;
        }

        void showContents(Model.Bar modelBar)
        {
            foreach (Chord chord in modelBar.chords)
            {
                newLabel(
                    lineY(6),
                    timeX(chord.time, LabelType.Chord),
                    chord.name,
                    LabelType.Chord
                    );
            }

            foreach (Note note in modelBar.notes)
            {
                newLabel(
                    lineY(note.line),
                    timeX(note.time, LabelType.Note),
                    note.fret.ToString(),
                    LabelType.Note
                    );
            }
        }

        static Brush backgroundBrush = Brushes.Ivory;

        static int lineY(int line)
        {
            return 150 - (20 * line);
        }

        double timeX(double time, LabelType labelType)
        {
            int beatsPerBar = state.beatsPerBar;
            double start = timeX(time, beatsPerBar);
            switch (labelType)
            {
                case LabelType.Chord:
                case LabelType.BarNumber:
                case LabelType.Comment:
                    return start;
                case LabelType.Note:
                    double span = (double)state.beatsPerBar / state.nIntervalsPerBar;
                    double next = timeX(time + span, beatsPerBar);
                    return (start + next) / 2;
                default:
                    throw new NotImplementedException();
            }
        }

        static double timeX(double time, int beatsPerBar)
        {
            return (180 * (time / beatsPerBar)) + 20;
        }

        void newLabel(double y, double x, string text, LabelType labelType)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;

            // set font size before measuring text
            switch (labelType)
            {
                case LabelType.Chord:
                case LabelType.Note:
                    // default size is 12
                    break;
                case LabelType.BarNumber:
                case LabelType.Comment:
                    textBlock.FontSize = 9;
                    break;
                default:
                    throw new NotImplementedException();
            }

            // https://stackoverflow.com/questions/9264398/how-to-calculate-wpf-textblock-width-for-its-known-font-size-and-characters
            textBlock.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            textBlock.Arrange(new Rect(textBlock.DesiredSize));
            double width = textBlock.ActualWidth;
            double height = textBlock.ActualHeight;

            switch (labelType)
            {
                case LabelType.Chord:
                    y -= (height / 2);

                    textBlock.Background = backgroundBrush;
                    break;

                case LabelType.Note:
                    y -= (height / 2);
                    x -= (width / 2);

                    textBlock.Background = backgroundBrush;
                    break;
                case LabelType.BarNumber:
                    y -= height;
                    x -= width;
                    textBlock.Foreground = Brushes.Gray;
                    break;
                case LabelType.Comment:
                    y -= height;
                    textBlock.Foreground = Brushes.Gray;
                    break;
            }

            Canvas.SetTop(textBlock, y);
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

        internal void TimerRemove()
        {
            if (cursor == null)
                throw new Exception();
            base.Children.Remove(cursor);
            cursor = null;
        }

        internal void TimerTick(int msecWithinBar, int msecBarDuration)
        {
            if (cursor == null)
            {
                cursor = newLine(0, 0, 50, 150, Brushes.Red);
            }

            SetLeft(cursor, timeTickX(msecWithinBar, msecBarDuration));
        }

        double timeTickX(int msecWithinBar, int msecBarDuration)
        {
            if (msecWithinBar == msecBarDuration)
            {
                msecWithinBar -= 10;
            }
            double fraction = (double)msecWithinBar / msecBarDuration;

            int interval = (int)(fraction * state.nIntervalsPerBar);
            double time = (double)interval / state.nIntervalsPerBeat;
            return timeX(time, LabelType.Note);
        }
    }
}
