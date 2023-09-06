using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("publish")]
[ExcludeFromCodeCoverage]
public record DotNetPublishOptions : DotNetBuildOptions
{
    [CommandEqualsSeparatorSwitch("--manifest", SwitchValueSeparator = " ")]
    public string? Manifest { get; init; }
}
