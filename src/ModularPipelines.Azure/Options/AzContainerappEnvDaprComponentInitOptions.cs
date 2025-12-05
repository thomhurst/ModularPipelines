using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "env", "dapr-component", "init")]
public record AzContainerappEnvDaprComponentInitOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--pubsub")]
    public string? Pubsub { get; set; }

    [CliOption("--statestore")]
    public string? Statestore { get; set; }
}