using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "env", "workload-profile", "list-supported")]
public record AzContainerappEnvWorkloadProfileListSupportedOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
}