using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicMaui.WebServices.Interfaces;
using Shared.Dtos;

namespace MusicMaui.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly IUserWebService _userWebService;

        public RegisterViewModel(IUserWebService userWebService)
        {
            _userWebService = userWebService;
        }

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private bool hasError;

        [ObservableProperty]
        private bool isLoading;

        [RelayCommand]
        public async Task Register()
        {
            HasError = false;
            IsLoading = true;

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ErrorMessage = "All fields are required.";
                HasError = true;
                IsLoading = false;
                return;
            }

            if (Password.Length < 8)
            {
                ErrorMessage = "Password must be at least 8 characters.";
                HasError = true;
                IsLoading = false;
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                HasError = true;
                IsLoading = false;
                return;
            }

            var registerDto = new LoginDto { Username = Username, Password = Password };
            var result = await _userWebService.Register(registerDto);

            IsLoading = false;

            if (result.Succeeded)
            {
                // Redirect to login screen after successful registration
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
                HasError = true;
            }
        }

        [RelayCommand]
        private async Task GoToLogin()
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
