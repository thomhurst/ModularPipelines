using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "deploy")]
public record GcloudAppDeployOptions(
[property: PositionalArgument] string Deployables
) : GcloudOptions
{
    [CommandSwitch("--appyaml")]
    public string? Appyaml { get; set; }

    [CommandSwitch("--bucket")]
    public string? Bucket { get; set; }

    [BooleanCommandSwitch("--cache")]
    public bool? Cache { get; set; }

    [CommandSwitch("--ignore-file")]
    public string? IgnoreFile { get; set; }

    [CommandSwitch("--image-url")]
    public string? ImageUrl { get; set; }

    [BooleanCommandSwitch("--promote")]
    public bool? Promote { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [BooleanCommandSwitch("--stop-previous-version")]
    public bool? StopPreviousVersion { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }
}