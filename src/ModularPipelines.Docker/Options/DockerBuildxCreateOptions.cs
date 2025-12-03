using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("buildx", "create")]
[ExcludeFromCodeCoverage]
public record DockerBuildxCreateOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? ContextOrEndpoint { get; set; }

    [CliOption("--append")]
    public virtual string? Append { get; set; }

    [CliOption("--bootstrap")]
    public virtual string? Bootstrap { get; set; }

    [CliOption("--builder")]
    public virtual string? Builder { get; set; }

    [CliOption("--buildkitd-flags")]
    public virtual string? BuildkitdFlags { get; set; }

    [CliOption("--driver")]
    public virtual string? Driver { get; set; }

    [CliOption("--driver-opt")]
    public virtual string? DriverOpt { get; set; }

    [CliOption("--leave")]
    public virtual string? Leave { get; set; }

    [CliOption("--name")]
    public virtual string? Name { get; set; }

    [CliOption("--node")]
    public virtual string? Node { get; set; }

    [CliOption("--platform")]
    public virtual string? Platform { get; set; }

    [CliOption("--use")]
    public virtual string? Use { get; set; }
}
