using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment", "mg", "delete")]
public record AzDeploymentMgDeleteOptions(
[property: CliOption("--management-group-id")] string ManagementGroupId,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}