using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicMaui.Services;
using MusicMaui.WebServices.Interfaces;
using Shared.Dtos;
using System.Collections.ObjectModel;

namespace MusicMaui.ViewModels
{
    public partial class AlbumDetailsViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IAlbumWebService _albumWebService;
        private readonly IReviewWebService _reviewWebService;
        private readonly DataHoldingService _dataHoldingService;
        private readonly ImageConversionService _imageConversionService;

        public AlbumDetailsViewModel(IAlbumWebService albumWebService, IReviewWebService reviewWebService, DataHoldingService dataHoldingService, ImageConversionService imageConversionService)
        {
            _albumWebService = albumWebService;
            _reviewWebService = reviewWebService;
            _dataHoldingService = dataHoldingService;
            _imageConversionService = imageConversionService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("albumId") && int.TryParse(query["albumId"].ToString(), out int id))
            {
                LoadAlbumDetails(id);
            }
        }

        [ObservableProperty] private bool isLoading;
        [ObservableProperty] private int albumId;
        [ObservableProperty] private string albumName;
        [ObservableProperty] private string artist;
        [ObservableProperty] private ImageSource image;
        [ObservableProperty] private int year;
        [ObservableProperty] private double averageScore;
        [ObservableProperty] private int reviewCount;
        [ObservableProperty] private ObservableCollection<ReviewDto> reviews = new();
        [ObservableProperty] private string reviewMessage;
        [ObservableProperty] private int reviewValue = 5;
        [ObservableProperty] private bool canSubmitReview;

        public async Task LoadAlbumDetails(int id)
        {
            IsLoading = true;
            AlbumId = id;

            var albumResult = await _albumWebService.Get(id);
            if (albumResult.Succeeded)
            {
                var album = albumResult.Data;
                AlbumName = album.Name;
                Artist = album.Artist;
                Image = _imageConversionService.ConvertBase64ToImageSource(album.Image);
                Year = album.Year;
                AverageScore = album.AverageScore;
                ReviewCount = album.ReviewCount;
                Reviews = new ObservableCollection<ReviewDto>(album.Reviews);
            }
            else
            {
                Console.WriteLine($"Failed to fetch album: {albumResult.ErrorMessage}");
            }

            CanSubmitReview = !string.IsNullOrEmpty(_dataHoldingService.Token);
            IsLoading = false;
        }

        [RelayCommand]
        private async Task SubmitReview()
        {
            if (!CanSubmitReview || string.IsNullOrWhiteSpace(ReviewMessage))
                return;

            var newReview = new NewReviewDto
            {
                AlbumId = AlbumId,
                Value = ReviewValue,
                Message = ReviewMessage
            };

            var result = await _reviewWebService.Create(newReview);
            if (result.Succeeded)
            {
                Console.WriteLine("Review added successfully. Reloading album details...");
                await LoadAlbumDetails(AlbumId);
                ReviewMessage = string.Empty;
            }
            else
            {
                Console.WriteLine($"Error submitting review: {result.ErrorMessage}");
            }
        }

        [RelayCommand]
        private async Task GoToUserProfile(int userId)
        {
            if (userId > 0)
            {
                await Shell.Current.GoToAsync($"UserProfilePage?userId={userId}");
            }
        }
    }
}

