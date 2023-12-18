using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("relay", "hyco", "authorization-rule", "create")]
public record AzRelayHycoAuthorizationRuleCreateOptions(
[property: CommandSwitch("--hybrid-connection-name")] string HybridConnectionName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--rights")]
    public string? Rights { get; set; }
}