using MusicMaui.ViewModels;

namespace MusicMaui.Views
{
    public partial class LogoutPage : ContentPage
    {
        private readonly LogoutViewModel _viewModel;

        public LogoutPage(LogoutViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Call logout logic
            await _viewModel.LogoutAsync();

            // Navigate to login page and REMOVE LogoutPage from stack
            await Shell.Current.GoToAsync("LoginPage");
        }
    }
}
