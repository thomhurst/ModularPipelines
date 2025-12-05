using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "describe-access-entry")]
public record AwsEksDescribeAccessEntryOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--principal-arn")] string PrincipalArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}