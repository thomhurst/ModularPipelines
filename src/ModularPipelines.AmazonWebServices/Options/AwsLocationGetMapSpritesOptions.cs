using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("location", "get-map-sprites")]
public record AwsLocationGetMapSpritesOptions(
[property: CliOption("--file-name")] string FileName,
[property: CliOption("--map-name")] string MapName
) : AwsOptions
{
    [CliOption("--key")]
    public string? Key { get; set; }
}