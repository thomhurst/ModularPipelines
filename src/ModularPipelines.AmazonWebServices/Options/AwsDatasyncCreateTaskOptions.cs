using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "create-task")]
public record AwsDatasyncCreateTaskOptions(
[property: CliOption("--source-location-arn")] string SourceLocationArn,
[property: CliOption("--destination-location-arn")] string DestinationLocationArn
) : AwsOptions
{
    [CliOption("--cloud-watch-log-group-arn")]
    public string? CloudWatchLogGroupArn { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--options")]
    public string? Options { get; set; }

    [CliOption("--excludes")]
    public string[]? Excludes { get; set; }

    [CliOption("--schedule")]
    public string? Schedule { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--includes")]
    public string[]? Includes { get; set; }

    [CliOption("--task-report-config")]
    public string? TaskReportConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}