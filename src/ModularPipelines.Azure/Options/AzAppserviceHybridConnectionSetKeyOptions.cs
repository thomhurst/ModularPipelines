using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appservice", "hybrid-connection", "set-key")]
public record AzAppserviceHybridConnectionSetKeyOptions(
[property: CommandSwitch("--hybrid-connection")] string HybridConnection,
[property: CommandSwitch("--key-type")] string KeyType,
[property: CommandSwitch("--namespace")] string Namespace,
[property: CommandSwitch("--plan")] string Plan,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}

