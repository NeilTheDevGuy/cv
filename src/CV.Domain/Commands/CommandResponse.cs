namespace CV.Domain.Commands;

public class CommandResponse
{
    public string? PreWriteStaticText { get; set; } //Static text that will display before writing
    public string? PostWriteStaticText { get; set; } //Static text that will display after writing
    public string? SlowText { get; set; } //The delayed-writing text.
}
