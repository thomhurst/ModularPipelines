using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("provider", "list")]
public record AzProviderListOptions : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }
}