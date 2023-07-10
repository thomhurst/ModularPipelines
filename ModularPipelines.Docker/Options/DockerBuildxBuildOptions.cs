using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx build")]
public record DockerBuildxBuildOptions : DockerOptions
{
    [CommandLongSwitch("detach")]
    public string? Detach { get; set; }

    [CommandLongSwitch("iidfile")]
    public string? Iidfile { get; set; }

    [CommandLongSwitch("invoke")]
    public string? Invoke { get; set; }

    [CommandLongSwitch("label")]
    public string? Label { get; set; }

    [CommandLongSwitch("network")]
    public string? Network { get; set; }

    [CommandLongSwitch("no-cache")]
    public string? NoCache { get; set; }

    [CommandLongSwitch("no-cache-filter")]
    public string? NoCacheFilter { get; set; }

    [CommandLongSwitch("print")]
    public string? Print { get; set; }

    [CommandLongSwitch("pull")]
    public string? Pull { get; set; }

    [CommandLongSwitch("quiet")]
    public string? Quiet { get; set; }

    [CommandLongSwitch("root")]
    public string? Root { get; set; }

    [CommandLongSwitch("server-config")]
    public string? ServerConfig { get; set; }

}