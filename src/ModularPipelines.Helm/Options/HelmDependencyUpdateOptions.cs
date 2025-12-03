using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("dependency", "update")]
[ExcludeFromCodeCoverage]
public record HelmDependencyUpdateOptions : HelmOptions
{
    [CliOption("--keyring")]
    public string? Keyring { get; set; }

    [CliFlag("--skip-refresh")]
    public virtual bool? SkipRefresh { get; set; }

    [CliFlag("--verify")]
    public virtual bool? Verify { get; set; }
}