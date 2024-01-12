using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("init")]
public record GcloudInitOptions : GcloudOptions
{
    [BooleanCommandSwitch("--no-browser")]
    public bool? NoBrowser { get; set; }

    [BooleanCommandSwitch("--no-launch-browser")]
    public bool? NoLaunchBrowser { get; set; }

    [BooleanCommandSwitch("--skip-diagnostics")]
    public bool? SkipDiagnostics { get; set; }
}