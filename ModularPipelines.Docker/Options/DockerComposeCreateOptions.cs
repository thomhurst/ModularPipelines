using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose create")]
public record DockerComposeCreateOptions : DockerOptions
{
    [CommandLongSwitch("build")]
    public string? Build { get; set; }

    [CommandLongSwitch("force-recreate")]
    public string? ForceRecreate { get; set; }

    [CommandLongSwitch("no-build")]
    public string? NoBuild { get; set; }

    [CommandLongSwitch("no-recreate")]
    public string? NoRecreate { get; set; }

    [CommandLongSwitch("pull")]
    public string? Pull { get; set; }

    [CommandLongSwitch("remove-orphans")]
    public string? RemoveOrphans { get; set; }

    [CommandLongSwitch("scale")]
    public string? Scale { get; set; }

    [CommandLongSwitch("dry-run")]
    public string? DryRun { get; set; }

}