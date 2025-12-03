using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment", "sub", "cancel")]
public record AzDeploymentSubCancelOptions(
[property: CliOption("--name")] string Name
) : AzOptions;