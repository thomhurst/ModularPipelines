using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "admin", "pool", "update")]
public record AzDevcenterAdminPoolUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--devbox-definition-name")]
    public string? DevboxDefinitionName { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--local-administrator")]
    public string? LocalAdministrator { get; set; }

    [CommandSwitch("--managed-virtual-network-regions")]
    public string? ManagedVirtualNetworkRegions { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--network-connection-name")]
    public string? NetworkConnectionName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--single-sign-on-status")]
    public string? SingleSignOnStatus { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--virtual-network-type")]
    public string? VirtualNetworkType { get; set; }
}