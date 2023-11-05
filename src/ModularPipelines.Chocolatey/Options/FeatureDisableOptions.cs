using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("feature", "disable")]
public record FeatureDisableOptions : ChocoOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }
}