using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp", "config", "access-restriction", "remove")]
public record AzFunctionappConfigAccessRestrictionRemoveOptions : AzOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

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
}