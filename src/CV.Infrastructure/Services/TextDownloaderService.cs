using CV.Application.Interfaces;

namespace CV.Infrastructure.Services;

public class TextDownloaderService : ITextDownloaderService
{
    private readonly HttpClient _httpClient;
    private const string _textLocationFolder = "text";

    public TextDownloaderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetText(string fileName)
    {
        var response = await _httpClient.GetAsync($"{_textLocationFolder}/{fileName}");
        var text = await response.Content.ReadAsStringAsync();
        return text;
    }
}
