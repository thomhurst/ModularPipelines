using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "deregister-rds-db-instance")]
public record AwsOpsworksDeregisterRdsDbInstanceOptions(
[property: CommandSwitch("--rds-db-instance-arn")] string RdsDbInstanceArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}