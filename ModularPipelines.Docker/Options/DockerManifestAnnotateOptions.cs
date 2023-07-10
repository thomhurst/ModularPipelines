using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("manifest annotate")]
public record DockerManifestAnnotateOptions : DockerOptions
{
    [CommandLongSwitch("arch")]
    public string? Arch { get; set; }

    [CommandLongSwitch("os")]
    public string? Os { get; set; }

    [CommandLongSwitch("os-features")]
    public string? OsFeatures { get; set; }

    [CommandLongSwitch("os-version")]
    public string? OsVersion { get; set; }

    [CommandLongSwitch("variant")]
    public string? Variant { get; set; }

}