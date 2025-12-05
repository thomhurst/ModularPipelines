using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconvert", "update-job-template")]
public record AwsMediaconvertUpdateJobTemplateOptions(
[property: CliOption("--name")] string Name
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

    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--status-update-interval")]
    public string? StatusUpdateInterval { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}