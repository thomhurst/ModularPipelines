using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "files", "list")]
public record GcloudArtifactsFilesListOptions : GcloudOptions
{
    [CommandSwitch("--package")]
    public string? Package { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }
}