using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "login")]
public record AzDevopsLoginOptions : AzOptions
{
    [CommandSwitch("--org")]
    public string? Org { get; set; }
}