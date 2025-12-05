using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("deployment", "sub", "show")]
public record AzDeploymentSubShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;