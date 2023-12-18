using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("feature", "list")]
public record AzFeatureListOptions : AzOptions
{
    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }
}