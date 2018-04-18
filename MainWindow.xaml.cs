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

using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using Microsoft.Win32;

using System.Diagnostics;

using Guitab.View;

namespace Guitab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // code and XAML copied from http://www.wpf-tutorial.com/audio-video/how-to-creating-a-complete-audio-video-player/

        private bool mediaPlayerIsPlaying = false;
        private bool userIsDraggingSlider = false;

        Stopwatch stopwatch = new Stopwatch();
        //bool playing;

        public MainWindow()
        {
            InitializeComponent();

            //ellapsed = new TimeSpan();

            // 170 bpm = 3 per second sec
            // * 4 to cope with quarter notes
            // * 10 for accuracy
            // => 120 second
            // DispatcherTimer precision is about 20 msec so 50 / second
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

            tempo.IntegerValue = 170;
            tempo.TextChanged += Tempo_TextChanged;
        }

        private void Tempo_TextChanged(object sender, TextChangedEventArgs e)
        {
            setDuration();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!mediaPlayerIsPlaying)
                return;
            long msec = stopwatch.ElapsedMilliseconds;
            viewBars.TimerTick(msec);
            //if ((mePlayer.Source != null) && (mePlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider))
            //{
            //    sliProgress.Minimum = 0;
            //    sliProgress.Maximum = mePlayer.NaturalDuration.TimeSpan.TotalSeconds;
            //    sliProgress.Value = mePlayer.Position.TotalSeconds;
            //}
        }

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*";
            //if (openFileDialog.ShowDialog() == true)
            //    mePlayer.Source = new Uri(openFileDialog.FileName);
            Model.Bars modelBars = Model.InputFile.Load();
            viewBars.Load(modelBars);

            sliProgress.Minimum = 0;
            sliProgress.Value = 0;
            setDuration();
        }

        void setDuration()
        {
            double oldMaximum = sliProgress.Maximum;
            sliProgress.Maximum = viewBars.getMsec(beatsPerMinute);
            if (oldMaximum != 0 && sliProgress.Value != 0)
            {
                sliProgress.Value = sliProgress.Value * sliProgress.Maximum / oldMaximum;
            }
        }

        int beatsPerMinute
        {
            get { return tempo.IntegerValue; }
        }

        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (viewBars != null) && viewBars.HasLoadedBars;
        }

        private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mediaPlayerIsPlaying = true;
            stopwatch.Start();
        }

        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // to do: add "and not at end of bars"
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mediaPlayerIsPlaying = false;
        }

        private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // to do: same as Pause_CanExecute
            e.CanExecute = mediaPlayerIsPlaying;
        }

        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mediaPlayerIsPlaying = false;
            sliProgress.Value = 0;
        }

        private void sliProgress_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDraggingSlider = true;
        }

        private void sliProgress_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDraggingSlider = false;
            //int msec = sliProgress.Value;
            //mePlayer.Position = TimeSpan.FromSeconds(sliProgress.Value);
        }

        private void sliProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblProgressStatus.Text = TimeSpan.FromMilliseconds(sliProgress.Value).ToString(@"hh\:mm\:ss");
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //mePlayer.Volume += (e.Delta > 0) ? 0.1 : -0.1;
        }

    }
}
