using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment", "tenant", "delete")]
public record AzDeploymentTenantDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}