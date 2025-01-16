using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicMaui.Services;
using MusicMaui.WebServices.Interfaces;
using Shared.Dtos;

namespace MusicMaui.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IUserWebService _userWebService;
        private readonly DataHoldingService _dataHoldingService;

        public LoginViewModel(IUserWebService userWebService, DataHoldingService dataHoldingService)
        {
            _userWebService = userWebService;
            _dataHoldingService = dataHoldingService;
        }

        [ObservableProperty]
        private string username;
        [ObservableProperty]
        private string password;
        [ObservableProperty]
        private string errorMessage;
        [ObservableProperty]
        private bool hasError;

        [RelayCommand]
        public async Task Login()
        {
            HasError = false;

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Username and password are required.";
                HasError = true;
                return;
            }

            var loginDto = new LoginDto { Username = Username, Password = Password };
            var result = await _userWebService.Login(loginDto);
            if (result.Succeeded)
            {
                Username = "";
                Password = "";
                ErrorMessage = "";
                HasError = false;

                var data = result.Data;
                await _dataHoldingService.SetTokenAsync(data.Jwt, data.ExpiryDate);
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
                HasError = true;
            }
        }

        [RelayCommand]
        private async Task GoToRegister()
        {
            await Shell.Current.GoToAsync("RegisterPage");
        }
    }
}
