using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("relay", "hyco", "authorization-rule", "create")]
public record AzRelayHycoAuthorizationRuleCreateOptions(
[property: CliOption("--hybrid-connection-name")] string HybridConnectionName,
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--rights")]
    public string? Rights { get; set; }
}