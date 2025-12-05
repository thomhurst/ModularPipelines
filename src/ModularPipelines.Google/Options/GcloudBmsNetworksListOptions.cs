using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "networks", "list")]
public record GcloudBmsNetworksListOptions : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}