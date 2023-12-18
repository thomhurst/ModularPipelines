using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "admin", "pool", "create")]
public record AzDevcenterAdminPoolCreateOptions(
[property: CommandSwitch("--devbox-definition-name")] string DevboxDefinitionName,
[property: CommandSwitch("--local-administrator")] string LocalAdministrator,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--project")] string Project,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--managed-virtual-network-regions")]
    public string? ManagedVirtualNetworkRegions { get; set; }

    [CommandSwitch("--network-connection-name")]
    public string? NetworkConnectionName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--single-sign-on-status")]
    public string? SingleSignOnStatus { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--virtual-network-type")]
    public string? VirtualNetworkType { get; set; }
}

