using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("feature", "enable")]
public record FeatureEnableOptions : ChocoOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }
}