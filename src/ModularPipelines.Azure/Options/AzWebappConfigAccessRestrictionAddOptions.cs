using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "config", "access-restriction", "add")]
public record AzWebappConfigAccessRestrictionAddOptions(
[property: CliOption("--priority")] string Priority
) : AzOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--http-headers")]
    public string? HttpHeaders { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--ignore-missing-endpoint")]
    public bool? IgnoreMissingEndpoint { get; set; }

    [CliOption("--ip-address")]
    public string? IpAddress { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rule-name")]
    public string? RuleName { get; set; }

    [CliFlag("--scm-site")]
    public bool? ScmSite { get; set; }

    [CliOption("--service-tag")]
    public string? ServiceTag { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }

    [CliOption("--vnet-resource-group")]
    public string? VnetResourceGroup { get; set; }
}