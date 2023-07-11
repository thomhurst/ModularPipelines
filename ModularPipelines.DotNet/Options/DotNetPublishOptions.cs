using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("publish")]
public record DotNetPublishOptions : DotNetBuildOptions
{
    [CommandEqualsSeparatorSwitch("--manifest", SwitchValueSeparator = " ")]
    public string? Manifest { get; init; }
}
