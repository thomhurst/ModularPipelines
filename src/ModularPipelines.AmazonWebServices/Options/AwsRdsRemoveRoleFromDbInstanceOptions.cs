using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "remove-role-from-db-instance")]
public record AwsRdsRemoveRoleFromDbInstanceOptions(
[property: CliOption("--db-instance-identifier")] string DbInstanceIdentifier,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--feature-name")] string FeatureName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}