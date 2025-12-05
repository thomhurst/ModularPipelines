using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("get", "all")]
[ExcludeFromCodeCoverage]
public record HelmGetAllOptions : HelmOptions
{
    [CliOption("--revision")]
    public virtual int? Revision { get; set; }

    [CliOption("--template")]
    public virtual string? Template { get; set; }
}