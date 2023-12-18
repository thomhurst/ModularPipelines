using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("search", "shared-private-link-resource", "update")]
public record AzSearchSharedPrivateLinkResourceUpdateOptions(
[property: CommandSwitch("--group-id")] string GroupId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--request-message")] string RequestMessage,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}