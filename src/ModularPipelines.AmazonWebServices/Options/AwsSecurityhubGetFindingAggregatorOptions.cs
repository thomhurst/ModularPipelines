using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "get-finding-aggregator")]
public record AwsSecurityhubGetFindingAggregatorOptions(
[property: CliOption("--finding-aggregator-arn")] string FindingAggregatorArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}