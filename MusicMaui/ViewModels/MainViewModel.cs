using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicMaui.DisplayModel;
using MusicMaui.Services;
using MusicMaui.WebServices.Interfaces;
using Shared.Dtos;
using System.Collections.ObjectModel;

namespace MusicMaui.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IAlbumWebService _albumWebService;
        private readonly ImageConversionService _imageConversionService;
        private int _currentPage = 1;
        private const int _pageSize = 5;
        private bool _isLastPage = false;

        public MainViewModel(IAlbumWebService albumWebService, ImageConversionService imageConversionService)
        {
            _albumWebService = albumWebService;
            _imageConversionService = imageConversionService;
            Albums = new ObservableCollection<AlbumDisplay>();
            SearchQuery = string.Empty; // Default empty search
            LoadMoreAlbums().ConfigureAwait(false);
        }

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private ObservableCollection<AlbumDisplay> albums;

        [ObservableProperty]
        private string searchQuery;

        [RelayCommand]
        private async Task LoadMoreAlbums()
        {
            if (_isLastPage || IsLoading)
            {
                return;
            }

            IsLoading = true;

            var request = new AlbumPagedRequest
            {
                PageNumber = _currentPage,
                PageSize = _pageSize,
                SortProperty = "AverageScore",
                SortOrder = "ascending",
                SearchQuery = SearchQuery
            };

            var result = await _albumWebService.GetPaged(request);

            if (result.Succeeded)
            {
                if (result.Data.Count < _pageSize)
                {
                    _isLastPage = true;
                }

                foreach (var album in result.Data)
                {
                    if (album == null)
                    {
                        continue;
                    }

                    var albumDisplay = new AlbumDisplay
                    {
                        Id = album.Id,
                        Name = album.Name,
                        Artist = album.Artist,
                        Image = _imageConversionService.ConvertBase64ToImageSource(album.Image),
                        Year = album.Year,
                        AverageScore = album.AverageScore,
                        ReviewCount = album.ReviewCount
                    };

                    Albums.Add(albumDisplay);
                }

                _currentPage++;
            }
            else
            {
                Console.WriteLine($"Error loading albums: {result.ErrorMessage}");
            }

            IsLoading = false;
        }

        [RelayCommand]
        private async Task SearchAlbums()
        {
            if (IsLoading) return;

            _currentPage = 1;
            _isLastPage = false;
            Albums.Clear(); // Reset the list

            await LoadMoreAlbums();
        }

        [RelayCommand]
        private async Task OpenAlbumDetails(int albumId)
        {
            await Shell.Current.GoToAsync($"AlbumDetails?albumId={albumId}");
        }
    }
}
