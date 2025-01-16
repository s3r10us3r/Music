namespace MusicMaui.DisplayModel
{
    public class AlbumDisplay
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Artist { get; set; } = "";
        public ImageSource Image { get; set; } 
        public int Year { get; set; }
        public double AverageScore { get; set; }
        public int ReviewCount { get; set; }
    }
}
