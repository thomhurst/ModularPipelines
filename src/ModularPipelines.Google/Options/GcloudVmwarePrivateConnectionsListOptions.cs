using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "private-connections", "list")]
public record GcloudVmwarePrivateConnectionsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}