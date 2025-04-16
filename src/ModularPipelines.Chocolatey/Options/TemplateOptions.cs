using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("template")]
public record TemplateOptions : ChocoOptions
{
    [CommandSwitch("--name")]
    public virtual string? Name { get; set; }
}