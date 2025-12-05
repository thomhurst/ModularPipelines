using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("get", "notes")]
[ExcludeFromCodeCoverage]
public record HelmGetNotesOptions : HelmOptions
{
    [CliOption("--revision")]
    public virtual int? Revision { get; set; }
}