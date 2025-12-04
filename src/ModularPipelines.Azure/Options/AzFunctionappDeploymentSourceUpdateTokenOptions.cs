using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp", "deployment", "source", "update-token")]
public record AzFunctionappDeploymentSourceUpdateTokenOptions : AzOptions
{
    [CliOption("--git-token")]
    public string? GitToken { get; set; }
}