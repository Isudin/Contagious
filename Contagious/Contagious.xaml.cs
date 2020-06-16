using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Contagious
{
    public partial class MainWindow : Window
    {
        private bool _isPlaying = false;
        private bool _loop = false;
        private Uri _uri;
        private Queue<Uri> _queue = new Queue<Uri>();
        private readonly MediaPlayer _mediaPlayer = new MediaPlayer();
        ContagiousViewModel Cvm;

        public MainWindow()
        {
            InitializeComponent();
            Cvm = new ContagiousViewModel();
            DataContext = Cvm;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (!_isPlaying)
            {
                playStopIcon.Kind = PackIconKind.Pause;
                _mediaPlayer.Play();
                _mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
                _isPlaying = true;
            }
            else
            {
                playStopIcon.Kind = PackIconKind.Play;
                _mediaPlayer.Pause();

                _isPlaying = false;
            }
        }

        private void Backward_Click(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.Open(_uri);
            _mediaPlayer.Play();
            _isPlaying = true;
            playStopIcon.Kind = PackIconKind.Pause;
        }
        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            Skip();
        }

        private void ChangeVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            _mediaPlayer.Volume = (double)volumeSlider.Value;
        }

        private void PlayNow(object sender, EventArgs e)
        {
            var item = (sender as ListView).SelectedItem;
            if (item != null)
            {
                _queue.Clear();
                title.Content = ((Track)item).Title;
                artists.Content = ((Track)item).Artist;
                _uri = new Uri(((Track)item).Source, UriKind.Relative);
                _queue.Enqueue(_uri);
                Skip();
            }
        }

        private void PlayNow_ContextMenu(object sender, EventArgs e)
        {
            var item = ((((MenuItem)sender).Parent as ContextMenu).PlacementTarget as ListView).SelectedItem;
            if (item != null)
            {
                _queue.Clear();
                title.Content = ((Track)item).Title;
                artists.Content = ((Track)item).Artist;
                _uri = new Uri(((Track)item).Source, UriKind.Relative);
                _queue.Enqueue(_uri);
                Skip();
            }
        }

        private void AddToQueue(object sender, EventArgs e)
        {
            var item = ((((MenuItem)sender).Parent as ContextMenu).PlacementTarget as ListView).SelectedItem;
            if (item != null)
            {
                _uri = new Uri(((Track)item).Source, UriKind.Relative);
                _queue.Enqueue(_uri);
            }
        }

        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            Skip();
        }

        private void Skip()
        {
            if (_queue.Count > 0)
            {
                Uri playing;
                List<Track> items = (List<Track>)TracksListView.ItemsSource;
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].Source == _queue.Peek().ToString())
                    {
                        title.Content = (items[i]).Title;
                        artists.Content = (items[i]).Artist;
                    }
                }
                playing = _queue.Dequeue();
                if (_loop)
                {
                    _queue.Enqueue(playing);
                }
                _mediaPlayer.Open(playing);
                _mediaPlayer.Play();
                playStopIcon.Kind = PackIconKind.Pause;
                _isPlaying = true;
            }
            else
            {
                _mediaPlayer.Stop();
                playStopIcon.Kind = PackIconKind.Play;
                _isPlaying = false;
            }
        }

        private void Random_Click(object sender, RoutedEventArgs e)
        {
            List<Uri> shuffleList = new List<Uri>();
            Random rnd = new Random();

            for (int i = 0; i < _queue.Count(); i++)
            {
                shuffleList.Add(_queue.Dequeue());
            }

            int n = shuffleList.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                Uri value = shuffleList[k];
                shuffleList[k] = shuffleList[n];
                shuffleList[n] = value;
            }

            foreach (var item in shuffleList)
            {
                _queue.Enqueue(item);
            }
        }

        private void Replay_Click(object sender, RoutedEventArgs e)
        {

            _loop = !_loop;
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ContextMenu contextMenu = button.ContextMenu;
            contextMenu.PlacementTarget = button;
            contextMenu.IsOpen = true;
        }

        private void ChangeFolder(object sender, RoutedEventArgs e)
        {
            ChangeFolder folder = new ChangeFolder(Cvm);
            folder.Show();
        }

        private void OnSizeChange(object sender, EventArgs e)
        {
            if (ContagiousWindow.Width < 600)
            {
                Name.Visibility = Visibility.Collapsed;
            }
            else
            {
                Name.Visibility = Visibility.Visible;
            }
        }
    }
}