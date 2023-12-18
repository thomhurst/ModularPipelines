using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("template", "info")]
public record TemplateInfoOptions : ChocoOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }
}