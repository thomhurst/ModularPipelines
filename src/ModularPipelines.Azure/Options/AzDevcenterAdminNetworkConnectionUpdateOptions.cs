using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devcenter", "admin", "network-connection", "update")]
public record AzDevcenterAdminNetworkConnectionUpdateOptions : AzOptions
{
    [CliOption("--domain-name")]
    public string? DomainName { get; set; }

    [CliOption("--domain-password")]
    public string? DomainPassword { get; set; }

    [CliOption("--domain-username")]
    public string? DomainUsername { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--organization-unit")]
    public string? OrganizationUnit { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subnet-id")]
    public string? SubnetId { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}