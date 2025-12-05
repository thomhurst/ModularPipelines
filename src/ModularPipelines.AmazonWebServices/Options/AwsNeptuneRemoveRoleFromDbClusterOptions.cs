using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune", "remove-role-from-db-cluster")]
public record AwsNeptuneRemoveRoleFromDbClusterOptions(
[property: CliOption("--db-cluster-identifier")] string DbClusterIdentifier,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--feature-name")]
    public string? FeatureName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}