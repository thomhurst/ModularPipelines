using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "modify-cluster-iam-roles")]
public record AwsRedshiftModifyClusterIamRolesOptions(
[property: CliOption("--cluster-identifier")] string ClusterIdentifier
) : AwsOptions
{
    [CliOption("--add-iam-roles")]
    public string[]? AddIamRoles { get; set; }

    [CliOption("--remove-iam-roles")]
    public string[]? RemoveIamRoles { get; set; }

    [CliOption("--default-iam-role-arn")]
    public string? DefaultIamRoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}