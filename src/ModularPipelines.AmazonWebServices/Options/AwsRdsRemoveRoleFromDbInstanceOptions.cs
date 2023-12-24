using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "remove-role-from-db-instance")]
public record AwsRdsRemoveRoleFromDbInstanceOptions(
[property: CommandSwitch("--db-instance-identifier")] string DbInstanceIdentifier,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--feature-name")] string FeatureName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}