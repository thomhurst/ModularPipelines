using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "describe-vpc-connection")]
public record AwsQuicksightDescribeVpcConnectionOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--vpc-connection-id")] string VpcConnectionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}