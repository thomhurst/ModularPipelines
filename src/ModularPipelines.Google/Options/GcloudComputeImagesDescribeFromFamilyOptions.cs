using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "images", "describe-from-family")]
public record GcloudComputeImagesDescribeFromFamilyOptions(
[property: CliArgument] string ImageName
) : GcloudOptions
{
    [CliOption("--zone")]
    public string? Zone { get; set; }
}