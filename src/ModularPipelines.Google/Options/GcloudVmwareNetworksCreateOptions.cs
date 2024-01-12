using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "networks", "create")]
public record GcloudVmwareNetworksCreateOptions(
[property: PositionalArgument] string VmwareEngineNetwork,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--type")] string Type
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }
}