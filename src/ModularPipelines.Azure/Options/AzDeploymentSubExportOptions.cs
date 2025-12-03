using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment", "sub", "export")]
public record AzDeploymentSubExportOptions(
[property: CliOption("--name")] string Name
) : AzOptions;