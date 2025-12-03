using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "deregister-rds-db-instance")]
public record AwsOpsworksDeregisterRdsDbInstanceOptions(
[property: CliOption("--rds-db-instance-arn")] string RdsDbInstanceArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}