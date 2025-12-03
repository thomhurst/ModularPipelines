using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "register-rds-db-instance")]
public record AwsOpsworksRegisterRdsDbInstanceOptions(
[property: CliOption("--stack-id")] string StackId,
[property: CliOption("--rds-db-instance-arn")] string RdsDbInstanceArn,
[property: CliOption("--db-user")] string DbUser,
[property: CliOption("--db-password")] string DbPassword
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}