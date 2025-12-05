using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("manifest", "create")]
[ExcludeFromCodeCoverage]
public record DockerManifestCreateOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Manifest { get; set; }

    [CliOption("--amend")]
    public virtual string? Amend { get; set; }

    [CliOption("--insecure")]
    public virtual string? Insecure { get; set; }
}
