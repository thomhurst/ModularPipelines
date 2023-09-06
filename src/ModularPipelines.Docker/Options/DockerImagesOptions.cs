using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("images")]
[ExcludeFromCodeCoverage]
public record DockerImagesOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string? Repository { get; set; }
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--digests")]
    public bool? Digests { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [BooleanCommandSwitch("--no-trunc")]
    public bool? NoTrunc { get; set; }
}
