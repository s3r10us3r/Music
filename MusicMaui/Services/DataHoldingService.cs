using System.Net.Http.Headers;
using System.Text.Json;

namespace MusicMaui.Services
{
    public class DataHoldingService
    {
        private readonly HttpClient _httpClient;

        public string Token { get; private set; } = "";
        public DateTime TokenExpiry { get; private set; } = DateTime.MinValue;

        public DataHoldingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            LoadToken().ConfigureAwait(false); // Load the token on app startup
        }

        public async Task SetTokenAsync(string token, DateTime expiry)
        {
            Token = token;
            TokenExpiry = expiry;

            // Set Authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            // Save securely in SecureStorage
            var tokenData = new TokenStorage
            {
                Value = Token,
                Expiry = TokenExpiry
            };

            string json = JsonSerializer.Serialize(tokenData);
            await SecureStorage.SetAsync("jwt_token", json);
        }

        private async Task LoadToken()
        {
            try
            {
                string json = await SecureStorage.GetAsync("jwt_token");
                if (!string.IsNullOrEmpty(json))
                {
                    var tokenData = JsonSerializer.Deserialize<TokenStorage>(json);
                    if (tokenData != null)
                    {
                        Token = tokenData.Value;
                        TokenExpiry = tokenData.Expiry;

                        // If token is still valid, set it in HttpClient
                        if (IsTokenValid())
                        {
                            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                        }
                        else
                        {
                            await ClearTokenAsync(); // Token expired, clear it
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading token: " + ex.Message);
            }
        }

        public async Task ClearTokenAsync()
        {
            Token = "";
            TokenExpiry = DateTime.MinValue;
            _httpClient.DefaultRequestHeaders.Authorization = null;

            // Remove from SecureStorage
            SecureStorage.Remove("jwt_token");
        }

        public bool IsTokenValid()
        {
            return !string.IsNullOrEmpty(Token) && DateTime.UtcNow < TokenExpiry;
        }

        private class TokenStorage
        {
            public string Value { get; set; } = "";
            public DateTime Expiry { get; set; }
        }
    }
}
