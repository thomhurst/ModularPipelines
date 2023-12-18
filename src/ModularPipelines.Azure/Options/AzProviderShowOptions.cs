using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("provider", "show")]
public record AzProviderShowOptions(
[property: CommandSwitch("--namespace")] string Namespace
) : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }
}