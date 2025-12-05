using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "deploy")]
public record GcloudAppDeployOptions(
[property: CliArgument] string Deployables
) : GcloudOptions
{
    [CliOption("--appyaml")]
    public string? Appyaml { get; set; }

    [CliOption("--bucket")]
    public string? Bucket { get; set; }

    [CliFlag("--cache")]
    public bool? Cache { get; set; }

    [CliOption("--ignore-file")]
    public string? IgnoreFile { get; set; }

    [CliOption("--image-url")]
    public string? ImageUrl { get; set; }

    [CliFlag("--promote")]
    public bool? Promote { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliFlag("--stop-previous-version")]
    public bool? StopPreviousVersion { get; set; }

    [CliOption("--version")]
    public new string? Version { get; set; }
}