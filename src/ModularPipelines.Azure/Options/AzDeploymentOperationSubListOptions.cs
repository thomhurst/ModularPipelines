using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("deployment", "operation", "sub", "list")]
public record AzDeploymentOperationSubListOptions(
[property: CliOption("--name")] string Name
) : AzOptions;