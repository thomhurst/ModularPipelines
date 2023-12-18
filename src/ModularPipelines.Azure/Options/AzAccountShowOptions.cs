using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "show")]
public record AzAccountShowOptions : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }
}