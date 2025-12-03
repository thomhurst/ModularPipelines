using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventarc", "channels", "list")]
public record GcloudEventarcChannelsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}