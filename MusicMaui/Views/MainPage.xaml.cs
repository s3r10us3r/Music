using MusicMaui.ViewModels;

namespace MusicMaui.Views;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel viewModel)
	{
        BindingContext = viewModel;
        InitializeComponent();
	}
}