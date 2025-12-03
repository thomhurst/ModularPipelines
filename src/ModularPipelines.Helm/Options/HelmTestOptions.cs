using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("test")]
[ExcludeFromCodeCoverage]
public record HelmTestOptions : HelmOptions
{
    [CliOption("--filter")]
    public string[]? Filter { get; set; }

    [CliFlag("--logs")]
    public virtual bool? Logs { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}