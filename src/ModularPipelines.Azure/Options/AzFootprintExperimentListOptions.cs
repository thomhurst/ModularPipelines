using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("footprint", "experiment", "list")]
public record AzFootprintExperimentListOptions(
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;