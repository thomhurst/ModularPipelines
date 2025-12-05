using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("search", "shared-private-link-resource", "create")]
public record AzSearchSharedPrivateLinkResourceCreateOptions(
[property: CliOption("--group-id")] string GroupId,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--request-message")]
    public string? RequestMessage { get; set; }
}