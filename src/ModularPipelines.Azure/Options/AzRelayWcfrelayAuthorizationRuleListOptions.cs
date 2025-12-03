using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("relay", "wcfrelay", "authorization-rule", "list")]
public record AzRelayWcfrelayAuthorizationRuleListOptions(
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--relay-name")] string RelayName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }
}