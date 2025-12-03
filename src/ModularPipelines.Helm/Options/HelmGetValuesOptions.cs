using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("get", "values")]
[ExcludeFromCodeCoverage]
public record HelmGetValuesOptions : HelmOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliOption("--revision")]
    public int? Revision { get; set; }
}