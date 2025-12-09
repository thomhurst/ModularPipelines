using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventhubs", "namespace", "authorization-rule", "create")]
public record AzEventhubsNamespaceAuthorizationRuleCreateOptions(
[property: CliOption("--authorization-rule-name")] string AuthorizationRuleName,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--rights")]
    public string? Rights { get; set; }
}