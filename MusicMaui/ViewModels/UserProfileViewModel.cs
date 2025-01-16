using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicMaui.DisplayModel;
using MusicMaui.Services;
using MusicMaui.WebServices.Interfaces;
using Shared.Dtos;
using System.Collections.ObjectModel;

namespace MusicMaui.ViewModels
{
    public partial class UserProfileViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IUserWebService _userWebService;
        private readonly ImageConversionService _imageConversionService;

        public UserProfileViewModel(IUserWebService userWebService, ImageConversionService imageConversionService)
        {
            _userWebService = userWebService;
            _imageConversionService = imageConversionService;
        }

        [ObservableProperty] private bool isLoading;
        [ObservableProperty] private string username;
        [ObservableProperty] private int userId;
        [ObservableProperty] private ObservableCollection<ReviewAlbumPairDisplay> reviews = new();

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("userId") && int.TryParse(query["userId"].ToString(), out int id))
            {
                LoadUserProfile(id);
            }
        }

        [RelayCommand]
        public async Task LoadUserProfile(int id)
        {
            IsLoading = true;
            UserId = id;

            var result = await _userWebService.GetUser(id);
            if (result.Succeeded)
            {
                var user = result.Data;
                Username = user.Username;
                var displayReviews = user.ReviewAlbumPairs.Select(pair => new ReviewAlbumPairDisplay
                {
                    Album = new AlbumDisplay
                    {
                        Id = pair.Album.Id,
                        Name = pair.Album.Name,
                        Artist = pair.Album.Artist,
                        Image = _imageConversionService.ConvertBase64ToImageSource(pair.Album.Image),
                        Year = pair.Album.Year,
                        AverageScore = pair.Album.AverageScore
                    },
                    Review = pair.Review
                }).ToList();
                Reviews = new ObservableCollection<ReviewAlbumPairDisplay>(displayReviews);
            }
            else
            {
                Console.WriteLine($"Failed to load user profile: {result.ErrorMessage}");
            }

            IsLoading = false;
        }
    }
}
