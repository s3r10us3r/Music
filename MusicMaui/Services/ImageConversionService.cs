namespace MusicMaui.Services
{
    public class ImageConversionService
    {
        public ImageSource ConvertBase64ToImageSource(string base64)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64);
                return ImageSource.FromStream(() => new MemoryStream(imageBytes));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error converting base64 to ImageSource: {e.Message}");
                return null;
            }
        }
    }
}
