using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "deployment", "source", "update-token")]
public record AzWebappDeploymentSourceUpdateTokenOptions : AzOptions
{
    [CommandSwitch("--git-token")]
    public string? GitToken { get; set; }
}