using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventarc", "google-channels", "describe")]
public record GcloudEventarcGoogleChannelsDescribeOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}