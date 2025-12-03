using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "start-import")]
public record AwsCloudtrailStartImportOptions : AwsOptions
{
    [CliOption("--destinations")]
    public string[]? Destinations { get; set; }

    [CliOption("--import-source")]
    public string? ImportSource { get; set; }

    [CliOption("--start-event-time")]
    public long? StartEventTime { get; set; }

    [CliOption("--end-event-time")]
    public long? EndEventTime { get; set; }

    [CliOption("--import-id")]
    public string? ImportId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}