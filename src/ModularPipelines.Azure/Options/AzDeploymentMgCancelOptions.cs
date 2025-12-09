using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("deployment", "mg", "cancel")]
public record AzDeploymentMgCancelOptions(
[property: CliOption("--management-group-id")] string ManagementGroupId,
[property: CliOption("--name")] string Name
) : AzOptions;