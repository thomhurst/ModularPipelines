using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "sub", "list")]
public record AzDeploymentSubListOptions : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }
}