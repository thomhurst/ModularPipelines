using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliSubCommand("test")]
[ExcludeFromCodeCoverage]
public record HelmTestOptions : HelmOptions
{
    [CliOption("--filter")]
    public virtual string[]? Filter { get; set; }

    [CliFlag("--logs")]
    public virtual bool? Logs { get; set; }

    [CliOption("--timeout")]
    public virtual string? Timeout { get; set; }
}