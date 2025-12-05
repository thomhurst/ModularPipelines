using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventarc", "google-channels", "describe")]
public record GcloudEventarcGoogleChannelsDescribeOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}