using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devcenter", "admin", "network-connection", "create")]
public record AzDevcenterAdminNetworkConnectionCreateOptions(
[property: CliOption("--domain-join-type")] string DomainJoinType,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--subnet-id")] string SubnetId
) : AzOptions
{
    [CliOption("--domain-name")]
    public string? DomainName { get; set; }

    [CliOption("--domain-password")]
    public string? DomainPassword { get; set; }

    [CliOption("--domain-username")]
    public string? DomainUsername { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--networking-resource-group-name")]
    public string? NetworkingResourceGroupName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--organization-unit")]
    public string? OrganizationUnit { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}