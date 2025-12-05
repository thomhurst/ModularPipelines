using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediapackage", "list-harvest-jobs")]
public record AwsMediapackageListHarvestJobsOptions : AwsOptions
{
    [CliOption("--include-channel-id")]
    public string? IncludeChannelId { get; set; }

    [CliOption("--include-status")]
    public string? IncludeStatus { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}