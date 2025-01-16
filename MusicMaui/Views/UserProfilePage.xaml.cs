using MusicMaui.ViewModels;

namespace MusicMaui.Views;

public partial class UserProfilePage : ContentPage
{
	public UserProfilePage(UserProfileViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}