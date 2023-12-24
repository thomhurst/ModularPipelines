using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "create-blue-green-deployment")]
public record AwsRdsCreateBlueGreenDeploymentOptions(
[property: CommandSwitch("--blue-green-deployment-name")] string BlueGreenDeploymentName,
[property: CommandSwitch("--source")] string Source
) : AwsOptions
{
    [CommandSwitch("--target-engine-version")]
    public string? TargetEngineVersion { get; set; }

    [CommandSwitch("--target-db-parameter-group-name")]
    public string? TargetDbParameterGroupName { get; set; }

    [CommandSwitch("--target-db-cluster-parameter-group-name")]
    public string? TargetDbClusterParameterGroupName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--target-db-instance-class")]
    public string? TargetDbInstanceClass { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}