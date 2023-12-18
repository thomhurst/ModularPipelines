using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("search", "shared-private-link-resource", "delete")]
public record AzSearchSharedPrivateLinkResourceDeleteOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions
{
    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}