using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("search", "shared-private-link-resource", "update")]
public record AzSearchSharedPrivateLinkResourceUpdateOptions(
[property: CliOption("--group-id")] string GroupId,
[property: CliOption("--name")] string Name,
[property: CliOption("--request-message")] string RequestMessage,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}