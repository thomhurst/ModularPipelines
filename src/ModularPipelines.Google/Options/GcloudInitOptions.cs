using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("init")]
public record GcloudInitOptions : GcloudOptions
{
    [CliFlag("--no-browser")]
    public bool? NoBrowser { get; set; }

    [CliFlag("--no-launch-browser")]
    public bool? NoLaunchBrowser { get; set; }

    [CliFlag("--skip-diagnostics")]
    public bool? SkipDiagnostics { get; set; }
}