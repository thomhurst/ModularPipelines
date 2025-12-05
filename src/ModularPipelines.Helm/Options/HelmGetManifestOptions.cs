using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("get", "manifest")]
[ExcludeFromCodeCoverage]
public record HelmGetManifestOptions : HelmOptions
{
    [CliOption("--revision")]
    public virtual int? Revision { get; set; }
}