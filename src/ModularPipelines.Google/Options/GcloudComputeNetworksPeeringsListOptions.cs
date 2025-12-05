using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "peerings", "list")]
public record GcloudComputeNetworksPeeringsListOptions : GcloudOptions
{
    [CliOption("--network")]
    public string? Network { get; set; }
}