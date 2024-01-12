using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logging", "logs", "list")]
public record GcloudLoggingLogsListOptions : GcloudOptions
{
    [CommandSwitch("--bucket")]
    public string? Bucket { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--view")]
    public string? View { get; set; }
}