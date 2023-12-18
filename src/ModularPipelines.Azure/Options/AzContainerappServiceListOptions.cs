using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "service", "list")]
public record AzContainerappServiceListOptions(
[property: CommandSwitch("--environment")] string Environment,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;