using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("deployment", "mg", "list")]
public record AzDeploymentMgListOptions(
[property: CliOption("--management-group-id")] string ManagementGroupId
) : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }
}