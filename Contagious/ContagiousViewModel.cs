using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Contagious
{
    public class ContagiousViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string dir { get; set; } = @"C:\Users\natan\source\repos\Contagious\Muzyka";

        public ContagiousViewModel()
        {
            Refresh();
        }

        public void Refresh()
        {
            List<Track> tracksList = new List<Track>();
            string[] list = Directory.GetFiles(dir, "*.mp3");
            foreach (var track in list)
            {
                TagLib.File tagFile = TagLib.File.Create(track);
                tracksList.Add(GetSong(tagFile.Tag.Title, tagFile.Tag.Performers.ToList().FirstOrDefault(), track));
            }
            TracksList = tracksList;
        }

        public IEnumerable<Track> TracksList { get; set; }

        public Track GetSong(string title, string performers, string source)
        {
            return new Track
            {
                Title = title,
                Artist = performers,
                Source = source
            };
        }
    }
}
