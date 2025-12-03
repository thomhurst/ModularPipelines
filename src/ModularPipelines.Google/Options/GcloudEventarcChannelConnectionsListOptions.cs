using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventarc", "channel-connections", "list")]
public record GcloudEventarcChannelConnectionsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}