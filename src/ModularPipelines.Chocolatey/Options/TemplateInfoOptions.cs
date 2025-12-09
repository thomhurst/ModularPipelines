using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Chocolatey.Options;

[ExcludeFromCodeCoverage]
[CliCommand("template", "info")]
public record TemplateInfoOptions : ChocoOptions
{
    [CliOption("--name")]
    public virtual string? Name { get; set; }
}