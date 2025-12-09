using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkfabric", "acl", "create")]
public record AzNetworkfabricAclCreateOptions(
[property: CliOption("--configuration-type")] string ConfigurationType,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-name")] string ResourceName
) : AzOptions
{
    [CliOption("--acls-url")]
    public string? AclsUrl { get; set; }

    [CliOption("--annotation")]
    public string? Annotation { get; set; }

    [CliOption("--default-action")]
    public string? DefaultAction { get; set; }

    [CliOption("--dynamic-match-configurations")]
    public string? DynamicMatchConfigurations { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--match-configurations")]
    public string? MatchConfigurations { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}