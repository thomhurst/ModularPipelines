using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("private-link", "association", "create")]
public record AzPrivateLinkAssociationCreateOptions(
[property: CliOption("--management-group-id")] string ManagementGroupId,
[property: CliOption("--name")] string Name,
[property: CliOption("--privatelink")] string Privatelink,
[property: CliOption("--public-network-access")] string PublicNetworkAccess
) : AzOptions;