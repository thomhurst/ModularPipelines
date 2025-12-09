using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "describe-data-set-refresh-properties")]
public record AwsQuicksightDescribeDataSetRefreshPropertiesOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--data-set-id")] string DataSetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}