using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "addon", "vr", "create")]
public record AzVmwareAddonVrCreateOptions(
[property: CommandSwitch("--private-cloud")] string PrivateCloud,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vrs-count")] int VrsCount
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}