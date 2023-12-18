using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "login")]
public record AzDevopsLoginOptions : AzOptions
{
    [CommandSwitch("--org")]
    public string? Org { get; set; }
}