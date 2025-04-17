using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

namespace WebClient.Pages;

public class ApiModel : PageModel
{
    private readonly ILogger<ApiModel> _logger;
    private readonly HttpClient _http;

    public ApiModel(ILogger<ApiModel> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _http = httpClientFactory.CreateClient();
    }

    public string RawJson { get; set; } = default!;

    public async Task OnGet()
    {
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _http.GetAsync("https://localhost:44379/api");

        response.EnsureSuccessStatusCode();

        RawJson = await response.Content.ReadAsStringAsync();
    }
}
