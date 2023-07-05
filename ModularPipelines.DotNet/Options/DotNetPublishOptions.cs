using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

public record DotNetPublishOptions : DotNetBuildOptions
{
    [CommandLongSwitch( "manifest", SwitchValueSeparator = " " )]
    public string? Manifest { get; init; }
}
