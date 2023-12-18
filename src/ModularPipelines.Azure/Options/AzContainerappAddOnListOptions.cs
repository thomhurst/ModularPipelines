using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "add-on", "list")]
public record AzContainerappAddOnListOptions(
[property: CommandSwitch("--environment")] string Environment,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;