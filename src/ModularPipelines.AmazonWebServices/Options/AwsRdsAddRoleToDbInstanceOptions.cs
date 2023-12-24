using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "add-role-to-db-instance")]
public record AwsRdsAddRoleToDbInstanceOptions(
[property: CommandSwitch("--db-instance-identifier")] string DbInstanceIdentifier,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--feature-name")] string FeatureName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}