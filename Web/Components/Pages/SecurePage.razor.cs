using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace Web.Components.Pages
{
    public partial class SecurePage
    {
        private string message = "Memuat data...";

        protected override async Task OnInitializedAsync()
        {
            // Ambil token akses dari HttpContext
            var accessToken = await HttpContextAccessor.HttpContext!.GetTokenAsync("access_token");

            if (string.IsNullOrEmpty(accessToken))
            {
                message = "Token akses tidak ditemukan.";
                return;
            }

            // Buat HttpClient dan tambahkan header otorisasi
            var httpClient = HttpClientFactory.CreateClient("backendApi");
            httpClient.BaseAddress = new Uri("http://localhost:5283");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            try
            {
                // Panggil endpoint API yang dilindungi
                var response = await httpClient.GetAsync("/api/users");

                if (response.IsSuccessStatusCode)
                {
                    message = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    message = $"Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
                }
            }
            catch (Exception ex)
            {
                message = $"Terjadi kesalahan: {ex.Message}";
            }
        }
    }
}