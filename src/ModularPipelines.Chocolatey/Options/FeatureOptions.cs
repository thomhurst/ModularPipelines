using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("feature")]
public record FeatureOptions : ChocoOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }
}