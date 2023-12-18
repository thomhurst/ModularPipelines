using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloud", "show")]
public record AzCloudShowOptions : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }
}