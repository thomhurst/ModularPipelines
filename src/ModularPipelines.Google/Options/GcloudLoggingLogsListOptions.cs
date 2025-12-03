using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logging", "logs", "list")]
public record GcloudLoggingLogsListOptions : GcloudOptions
{
    [CliOption("--bucket")]
    public string? Bucket { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--view")]
    public string? View { get; set; }
}