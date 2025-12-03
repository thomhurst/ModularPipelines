using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "start-query")]
public record AwsCloudtrailStartQueryOptions : AwsOptions
{
    [CliOption("--query-statement")]
    public string? QueryStatement { get; set; }

    [CliOption("--delivery-s3-uri")]
    public string? DeliveryS3Uri { get; set; }

    [CliOption("--query-alias")]
    public string? QueryAlias { get; set; }

    [CliOption("--query-parameters")]
    public string[]? QueryParameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}