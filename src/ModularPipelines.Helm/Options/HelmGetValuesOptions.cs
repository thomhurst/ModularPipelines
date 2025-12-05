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
    public virtual string? Output { get; set; }

    [CliOption("--revision")]
    public virtual int? Revision { get; set; }
}