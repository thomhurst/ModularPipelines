using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("deployment", "mg", "delete")]
public record AzDeploymentMgDeleteOptions(
[property: CliOption("--management-group-id")] string ManagementGroupId,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}