using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("demo", "style")]
public record AzDemoStyleOptions : AzOptions
{
    [CommandSwitch("--theme")]
    public string? Theme { get; set; }
}