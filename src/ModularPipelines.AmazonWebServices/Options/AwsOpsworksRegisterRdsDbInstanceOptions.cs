using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "register-rds-db-instance")]
public record AwsOpsworksRegisterRdsDbInstanceOptions(
[property: CommandSwitch("--stack-id")] string StackId,
[property: CommandSwitch("--rds-db-instance-arn")] string RdsDbInstanceArn,
[property: CommandSwitch("--db-user")] string DbUser,
[property: CommandSwitch("--db-password")] string DbPassword
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}