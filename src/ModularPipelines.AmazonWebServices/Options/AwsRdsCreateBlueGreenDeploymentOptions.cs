using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "create-blue-green-deployment")]
public record AwsRdsCreateBlueGreenDeploymentOptions(
[property: CliOption("--blue-green-deployment-name")] string BlueGreenDeploymentName,
[property: CliOption("--source")] string Source
) : AwsOptions
{
    [CliOption("--target-engine-version")]
    public string? TargetEngineVersion { get; set; }

    [CliOption("--target-db-parameter-group-name")]
    public string? TargetDbParameterGroupName { get; set; }

    [CliOption("--target-db-cluster-parameter-group-name")]
    public string? TargetDbClusterParameterGroupName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--target-db-instance-class")]
    public string? TargetDbInstanceClass { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}