using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("dependency", "build")]
[ExcludeFromCodeCoverage]
public record HelmDependencyBuildOptions : HelmOptions
{
    [CliOption("--keyring")]
    public virtual string? Keyring { get; set; }

    [CliFlag("--skip-refresh")]
    public virtual bool? SkipRefresh { get; set; }

    [CliFlag("--verify")]
    public virtual bool? Verify { get; set; }
}