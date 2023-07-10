using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose config")]
public record DockerComposeConfigOptions : DockerOptions
{
    [CommandLongSwitch("format")]
    public string? Format { get; set; }

    [CommandLongSwitch("hash")]
    public string? Hash { get; set; }

    [CommandLongSwitch("images")]
    public string? Images { get; set; }

    [CommandLongSwitch("no-consistency")]
    public string? NoConsistency { get; set; }

    [CommandLongSwitch("no-interpolate")]
    public string? NoInterpolate { get; set; }

    [CommandLongSwitch("no-normalize")]
    public string? NoNormalize { get; set; }

    [CommandLongSwitch("no-path-resolution")]
    public string? NoPathResolution { get; set; }

    [CommandLongSwitch("output")]
    public string? Output { get; set; }

    [CommandLongSwitch("profiles")]
    public string? Profiles { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

    [CommandLongSwitch("resolve-image-digests")]
    public string? ResolveImageDigests { get; set; }

    [CommandLongSwitch("services")]
    public string? Services { get; set; }

    [CommandLongSwitch("volumes")]
    public string? Volumes { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}