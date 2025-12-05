using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "logs", "tail")]
public record GcloudAppLogsTailOptions : GcloudOptions
{
    [CliOption("--level")]
    public string? Level { get; set; }

    [CliOption("--logs")]
    public string[]? Logs { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }
}