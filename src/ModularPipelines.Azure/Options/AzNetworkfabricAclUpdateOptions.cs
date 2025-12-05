using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkfabric", "acl", "update")]
public record AzNetworkfabricAclUpdateOptions : AzOptions
{
    [CliOption("--acls-url")]
    public string? AclsUrl { get; set; }

    [CliOption("--annotation")]
    public string? Annotation { get; set; }

    [CliOption("--configuration-type")]
    public string? ConfigurationType { get; set; }

    [CliOption("--default-action")]
    public string? DefaultAction { get; set; }

    [CliOption("--dynamic-match-configurations")]
    public string? DynamicMatchConfigurations { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--match-configurations")]
    public string? MatchConfigurations { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-name")]
    public string? ResourceName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}