using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("deployment", "mg", "show")]
public record AzDeploymentMgShowOptions(
[property: CliOption("--management-group-id")] string ManagementGroupId,
[property: CliOption("--name")] string Name
) : AzOptions;