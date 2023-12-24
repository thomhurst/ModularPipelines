using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "update-rds-db-instance")]
public record AwsOpsworksUpdateRdsDbInstanceOptions(
[property: CommandSwitch("--rds-db-instance-arn")] string RdsDbInstanceArn
) : AwsOptions
{
    [CommandSwitch("--db-user")]
    public string? DbUser { get; set; }

    [CommandSwitch("--db-password")]
    public string? DbPassword { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}