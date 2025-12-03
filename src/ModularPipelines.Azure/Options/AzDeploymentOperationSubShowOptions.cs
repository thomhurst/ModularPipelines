using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment", "operation", "sub", "show")]
public record AzDeploymentOperationSubShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--operation-ids")] string OperationIds
) : AzOptions;