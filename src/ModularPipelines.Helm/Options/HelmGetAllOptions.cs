using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("get", "all")]
[ExcludeFromCodeCoverage]
public record HelmGetAllOptions : HelmOptions
{
    [CliOption("--revision")]
    public int? Revision { get; set; }

    [CliOption("--template")]
    public string? Template { get; set; }
}