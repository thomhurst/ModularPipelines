using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune", "add-role-to-db-cluster")]
public record AwsNeptuneAddRoleToDbClusterOptions(
[property: CommandSwitch("--db-cluster-identifier")] string DbClusterIdentifier,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--feature-name")]
    public string? FeatureName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}