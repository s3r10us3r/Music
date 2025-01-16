using MusicMaui.ViewModels;

namespace MusicMaui.Views;

public partial class AddAlbumPage : ContentPage
{
	public AddAlbumPage(AddAlbumViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}