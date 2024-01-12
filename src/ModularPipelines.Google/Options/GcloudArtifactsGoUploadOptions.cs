using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "go", "upload")]
public record GcloudArtifactsGoUploadOptions : GcloudOptions
{
    public GcloudArtifactsGoUploadOptions(
        string modulePath,
        string version
    )
    {
        ModulePath = modulePath;
        Version = version;
    }

    [CommandSwitch("--module-path")]
    public string ModulePath { get; set; }

    [CommandSwitch("--version")]
    public new string Version { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }
}
