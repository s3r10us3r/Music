using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicMaui.Services;
using MusicMaui.WebServices.Interfaces;
using Shared.Dtos;

namespace MusicMaui.ViewModels
{
    public partial class AddAlbumViewModel : ObservableObject
    {
        private readonly IAlbumWebService _albumWebService;
        private readonly ImageConversionService _imageConversionService;
        private readonly DataHoldingService _dataHoldingService;

        public AddAlbumViewModel(IAlbumWebService albumWebService,
                                 ImageConversionService imageConversionService,
                                 DataHoldingService dataHoldingService)
        {
            _albumWebService = albumWebService;
            _imageConversionService = imageConversionService;
            _dataHoldingService = dataHoldingService;
        }

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string artist;

        [ObservableProperty]
        private string year;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private bool hasError;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private ImageSource albumCover;

        private string base64Image = "";

        [ObservableProperty]
        private bool isImageSelected;

        [RelayCommand]
        private async Task PickImage()
        {
            try
            {
                var fileResult = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Select Album Cover"
                });

                if (fileResult != null)
                {
                    using var stream = await fileResult.OpenReadAsync();
                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);

                    base64Image = Convert.ToBase64String(memoryStream.ToArray());
                    AlbumCover = ImageSource.FromStream(() => new MemoryStream(memoryStream.ToArray()));
                    IsImageSelected = true;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to select image.";
                HasError = true;
            }
        }
        [RelayCommand]
        private async Task AddAlbum()
        {
            HasError = false;

            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Artist) || string.IsNullOrWhiteSpace(Year) || string.IsNullOrWhiteSpace(base64Image))
            {
                ErrorMessage = "All fields are required.";
                HasError = true;
                return;
            }

            if (!int.TryParse(Year, out int parsedYear))
            {
                ErrorMessage = "Invalid year format.";
                HasError = true;
                return;
            }

            IsLoading = true;

            var newAlbum = new NewAlbumDto
            {
                Name = Name,
                Artist = Artist,
                Year = parsedYear,
                Image = base64Image
            };

            var result = await _albumWebService.Create(newAlbum);

            if (result.Succeeded)
            {
                Name = "";
                HasError = false;
                Artist = "";
                Year = "";
                ErrorMessage = "";
                IsLoading = false;
                AlbumCover = null;
                isImageSelected = false;
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
                HasError = true;
            }

            IsLoading = false;
        }
    }
}
