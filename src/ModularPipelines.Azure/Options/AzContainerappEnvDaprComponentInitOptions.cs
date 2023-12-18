using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "env", "dapr-component", "init")]
public record AzContainerappEnvDaprComponentInitOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--pubsub")]
    public string? Pubsub { get; set; }

    [CommandSwitch("--statestore")]
    public string? Statestore { get; set; }
}