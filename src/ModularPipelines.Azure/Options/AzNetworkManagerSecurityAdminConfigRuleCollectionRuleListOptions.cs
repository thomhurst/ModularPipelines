using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "manager", "security-admin-config", "rule-collection", "rule", "list")]
public record AzNetworkManagerSecurityAdminConfigRuleCollectionRuleListOptions(
[property: CliOption("--configuration-name")] string ConfigurationName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule-collection-name")] string RuleCollectionName
) : AzOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}