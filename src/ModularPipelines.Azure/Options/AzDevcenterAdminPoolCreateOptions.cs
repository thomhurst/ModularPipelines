using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "admin", "pool", "create")]
public record AzDevcenterAdminPoolCreateOptions(
[property: CliOption("--devbox-definition-name")] string DevboxDefinitionName,
[property: CliOption("--local-administrator")] string LocalAdministrator,
[property: CliOption("--name")] string Name,
[property: CliOption("--project")] string Project,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managed-virtual-network-regions")]
    public string? ManagedVirtualNetworkRegions { get; set; }

    [CliOption("--network-connection-name")]
    public string? NetworkConnectionName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--single-sign-on-status")]
    public string? SingleSignOnStatus { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--virtual-network-type")]
    public string? VirtualNetworkType { get; set; }
}