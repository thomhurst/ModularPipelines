using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment", "tenant", "list")]
public record AzDeploymentTenantListOptions : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }
}