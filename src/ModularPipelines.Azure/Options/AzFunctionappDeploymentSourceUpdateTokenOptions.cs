using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "deployment", "source", "update-token")]
public record AzFunctionappDeploymentSourceUpdateTokenOptions : AzOptions
{
    [CommandSwitch("--git-token")]
    public string? GitToken { get; set; }
}