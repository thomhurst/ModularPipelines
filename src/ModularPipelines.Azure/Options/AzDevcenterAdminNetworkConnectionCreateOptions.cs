using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "admin", "network-connection", "create")]
public record AzDevcenterAdminNetworkConnectionCreateOptions(
[property: CommandSwitch("--domain-join-type")] string DomainJoinType,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--subnet-id")] string SubnetId
) : AzOptions
{
    [CommandSwitch("--domain-name")]
    public string? DomainName { get; set; }

    [CommandSwitch("--domain-password")]
    public string? DomainPassword { get; set; }

    [CommandSwitch("--domain-username")]
    public string? DomainUsername { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--networking-resource-group-name")]
    public string? NetworkingResourceGroupName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--organization-unit")]
    public string? OrganizationUnit { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}