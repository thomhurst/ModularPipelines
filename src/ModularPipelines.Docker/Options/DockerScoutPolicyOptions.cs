using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CliCommand("scout", "policy")]
[ExcludeFromCodeCoverage]
public record DockerScoutPolicyOptions : DockerOptions
{
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? ImageOrRepo { get; set; }

    [CliOption("--env")]
    public virtual string? Env { get; set; }

    [CliOption("--exit-code")]
    public virtual string? ExitCode { get; set; }

    [CliOption("--org")]
    public virtual string? Org { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliOption("--platform")]
    public virtual string? Platform { get; set; }

    [CliOption("--to-env")]
    public virtual string? ToEnv { get; set; }

    [CliOption("--to-latest")]
    public virtual string? ToLatest { get; set; }
}
