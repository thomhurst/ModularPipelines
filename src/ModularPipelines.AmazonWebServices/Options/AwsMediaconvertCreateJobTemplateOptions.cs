using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconvert", "create-job-template")]
public record AwsMediaconvertCreateJobTemplateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--settings")] string Settings
) : AwsOptions
{
    [CliOption("--acceleration-settings")]
    public string? AccelerationSettings { get; set; }

    [CliOption("--category")]
    public string? Category { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--hop-destinations")]
    public string[]? HopDestinations { get; set; }

    [CliOption("--priority")]
    public int? Priority { get; set; }

    [CliOption("--queue")]
    public string? Queue { get; set; }

    [CliOption("--status-update-interval")]
    public string? StatusUpdateInterval { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}