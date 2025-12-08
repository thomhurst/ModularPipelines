using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliSubCommand("version")]
[ExcludeFromCodeCoverage]
public record HelmVersionOptions : HelmOptions
{
    [CliFlag("--short")]
    public virtual bool? Short { get; set; }

    [CliOption("--template")]
    public virtual string? Template { get; set; }
}