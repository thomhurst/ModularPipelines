using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "peerings", "list")]
public record GcloudComputeNetworksPeeringsListOptions : GcloudOptions
{
    [CommandSwitch("--network")]
    public string? Network { get; set; }
}