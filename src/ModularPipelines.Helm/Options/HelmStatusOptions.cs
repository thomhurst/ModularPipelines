using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("status")]
[ExcludeFromCodeCoverage]
public record HelmStatusOptions : HelmOptions
{
    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliOption("--revision")]
    public virtual int? Revision { get; set; }

    [CliFlag("--show-desc")]
    public virtual bool? ShowDesc { get; set; }

    [CliFlag("--show-resources")]
    public virtual bool? ShowResources { get; set; }
}