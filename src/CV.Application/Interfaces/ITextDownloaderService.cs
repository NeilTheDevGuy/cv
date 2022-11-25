namespace CV.Application.Interfaces;

public interface ITextDownloaderService
{
    public Task<string> GetText(string fileName);
}
