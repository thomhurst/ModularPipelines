using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "update-rds-db-instance")]
public record AwsOpsworksUpdateRdsDbInstanceOptions(
[property: CliOption("--rds-db-instance-arn")] string RdsDbInstanceArn
) : AwsOptions
{
    [CliOption("--db-user")]
    public string? DbUser { get; set; }

    [CliOption("--db-password")]
    public string? DbPassword { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}