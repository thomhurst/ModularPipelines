using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "describe-query")]
public record AwsCloudtrailDescribeQueryOptions : AwsOptions
{
    [CliOption("--event-data-store")]
    public string? EventDataStore { get; set; }

    [CliOption("--query-id")]
    public string? QueryId { get; set; }

    [CliOption("--query-alias")]
    public string? QueryAlias { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}