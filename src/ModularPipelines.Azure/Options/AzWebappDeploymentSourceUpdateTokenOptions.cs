using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "deployment", "source", "update-token")]
public record AzWebappDeploymentSourceUpdateTokenOptions : AzOptions
{
    [CommandSwitch("--git-token")]
    public string? GitToken { get; set; }
}