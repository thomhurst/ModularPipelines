using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "manager", "create")]
public record AzNetworkManagerCreateOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--network-manager-scopes")] string NetworkManagerScopes,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--scope-accesses")] string ScopeAccesses
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}