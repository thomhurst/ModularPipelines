using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("get", "hooks")]
[ExcludeFromCodeCoverage]
public record HelmGetHooksOptions : HelmOptions
{
    [CliOption("--revision")]
    public int? Revision { get; set; }
}