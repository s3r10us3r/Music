using MusicMaui.ViewModels;

namespace MusicMaui.Views;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}