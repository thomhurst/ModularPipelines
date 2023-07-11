using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose logs")]
public record DockerComposeLogsOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public IEnumerable<string> Service { get; set; }
    [BooleanCommandSwitch("--follow")]
    public bool? Follow { get; set; }

    [BooleanCommandSwitch("--no-color")]
    public bool? NoColor { get; set; }

    [CommandSwitch("--no-log-prefix")]
    public string? NoLogPrefix { get; set; }

    [CommandSwitch("--since")]
    public string? Since { get; set; }

    [CommandSwitch("--tail")]
    public string? Tail { get; set; }

    [BooleanCommandSwitch("--timestamps")]
    public bool? Timestamps { get; set; }

    [CommandSwitch("--until")]
    public string? Until { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

}