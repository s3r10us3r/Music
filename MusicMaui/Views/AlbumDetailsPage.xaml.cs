using MusicMaui.ViewModels;

namespace MusicMaui.Views;

public partial class AlbumDetailsPage : ContentPage
{
	public AlbumDetailsPage(AlbumDetailsViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}