using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "images", "deprecate")]
public record GcloudComputeImagesDeprecateOptions(
[property: CliArgument] string ImageName,
[property: CliOption("--state")] string State
) : GcloudOptions
{
    [CliOption("--replacement")]
    public string? Replacement { get; set; }

    [CliOption("--delete-in")]
    public string? DeleteIn { get; set; }

    [CliOption("--delete-on")]
    public string? DeleteOn { get; set; }

    [CliOption("--deprecate-in")]
    public string? DeprecateIn { get; set; }

    [CliOption("--deprecate-on")]
    public string? DeprecateOn { get; set; }

    [CliOption("--obsolete-in")]
    public string? ObsoleteIn { get; set; }

    [CliOption("--obsolete-on")]
    public string? ObsoleteOn { get; set; }
}