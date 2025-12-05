using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "describe-rds-db-instances")]
public record AwsOpsworksDescribeRdsDbInstancesOptions(
[property: CliOption("--stack-id")] string StackId
) : AwsOptions
{
    [CliOption("--rds-db-instance-arns")]
    public string[]? RdsDbInstanceArns { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}