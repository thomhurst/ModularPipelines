using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "deployment", "source", "update-token")]
public record AzWebappDeploymentSourceUpdateTokenOptions : AzOptions
{
    [CliOption("--git-token")]
    public string? GitToken { get; set; }
}