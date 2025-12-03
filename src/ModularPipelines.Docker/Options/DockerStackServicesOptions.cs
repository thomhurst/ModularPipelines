using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerStackServicesOptions : DockerOptions
{
    public DockerStackServicesOptions(
        string stack
    )
    {
        CommandParts = ["stack", "services"];

        Stack = stack;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Stack { get; set; }

    [CliOption("--filter")]
    public virtual string? Filter { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}
