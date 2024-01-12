using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "networks", "list")]
public record GcloudVmwareNetworksListOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}