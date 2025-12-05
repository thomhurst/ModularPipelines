using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconvert", "create-preset")]
public record AwsMediaconvertCreatePresetOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--settings")] string Settings
) : AwsOptions
{
    [CliOption("--category")]
    public string? Category { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}