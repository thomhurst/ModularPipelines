using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "describe-rds-db-instances")]
public record AwsOpsworksDescribeRdsDbInstancesOptions(
[property: CommandSwitch("--stack-id")] string StackId
) : AwsOptions
{
    [CommandSwitch("--rds-db-instance-arns")]
    public string[]? RdsDbInstanceArns { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}