using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerServiceInspectOptions : DockerOptions
{
    public DockerServiceInspectOptions(
        IEnumerable<string> service
    )
    {
        CommandParts = ["service", "inspect"];

        Service = service;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual IEnumerable<string>? Service { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliOption("--pretty")]
    public virtual string? Pretty { get; set; }
}
