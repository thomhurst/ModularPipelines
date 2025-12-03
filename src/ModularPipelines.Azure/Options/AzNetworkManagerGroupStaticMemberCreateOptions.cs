using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "manager", "group", "static-member", "create")]
public record AzNetworkManagerGroupStaticMemberCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--network-group")] string NetworkGroup,
[property: CliOption("--network-manager")] string NetworkManager,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-id")] string ResourceId
) : AzOptions;