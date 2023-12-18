using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "sub", "show")]
public record AzDeploymentSubShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;