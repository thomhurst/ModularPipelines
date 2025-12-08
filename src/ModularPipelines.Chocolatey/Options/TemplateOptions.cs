using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("template")]
public record TemplateOptions : ChocoOptions
{
    [CliOption("--name")]
    public virtual string? Name { get; set; }
}