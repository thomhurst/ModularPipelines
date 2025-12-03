using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devcenter", "admin", "pool", "update")]
public record AzDevcenterAdminPoolUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--devbox-definition-name")]
    public string? DevboxDefinitionName { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--local-administrator")]
    public string? LocalAdministrator { get; set; }

    [CliOption("--managed-virtual-network-regions")]
    public string? ManagedVirtualNetworkRegions { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--network-connection-name")]
    public string? NetworkConnectionName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--single-sign-on-status")]
    public string? SingleSignOnStatus { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--virtual-network-type")]
    public string? VirtualNetworkType { get; set; }
}