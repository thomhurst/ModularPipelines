using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerConfigInspectOptions : DockerOptions
{
    public DockerConfigInspectOptions(
        IEnumerable<string> config
    )
    {
        CommandParts = ["config", "inspect"];

        InspectConfig = config;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? InspectConfig { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliOption("--pretty")]
    public virtual string? Pretty { get; set; }
}
