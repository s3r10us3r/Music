using MusicMaui.Views;

namespace MusicMaui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
            Routing.RegisterRoute("AlbumDetails", typeof(AlbumDetailsPage));
            Routing.RegisterRoute("UserProfilePage", typeof(UserProfilePage));
            Navigated += OnNavigated;
        }

        private void OnNavigated(object sender, ShellNavigatedEventArgs e)
        {
            // Disable Flyout for Login & Register pages
            if (e.Current.Location.OriginalString.Contains("LoginPage") ||
                e.Current.Location.OriginalString.Contains("RegisterPage"))
            {
                FlyoutBehavior = FlyoutBehavior.Disabled;
            }
            else
            {
                FlyoutBehavior = FlyoutBehavior.Flyout;
            }
        }
    }
}
