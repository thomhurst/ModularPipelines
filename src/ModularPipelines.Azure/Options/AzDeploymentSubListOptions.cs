using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deployment", "sub", "list")]
public record AzDeploymentSubListOptions : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }
}