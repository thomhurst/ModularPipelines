using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "cloud-link", "create")]
public record AzVmwareCloudLinkCreateOptions(
[property: CommandSwitch("--cloud-link-name")] string CloudLinkName,
[property: CommandSwitch("--linked-cloud")] string LinkedCloud,
[property: CommandSwitch("--private-cloud")] string PrivateCloud,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}