using CommunityToolkit.Mvvm.ComponentModel;
using MusicMaui.Services;
using System.Threading.Tasks;

namespace MusicMaui.ViewModels
{
    public partial class LogoutViewModel : ObservableObject
    {
        private readonly DataHoldingService _dataHoldingService;

        public LogoutViewModel(DataHoldingService dataHoldingService)
        {
            _dataHoldingService = dataHoldingService;
        }

        public async Task LogoutAsync()
        {
            SecureStorage.Remove("jwt");
            SecureStorage.Remove("jwt_expiry");
            await _dataHoldingService.ClearTokenAsync();
        }
    }
}
