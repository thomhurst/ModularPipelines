using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "modify-cluster-iam-roles")]
public record AwsRedshiftModifyClusterIamRolesOptions(
[property: CommandSwitch("--cluster-identifier")] string ClusterIdentifier
) : AwsOptions
{
    [CommandSwitch("--add-iam-roles")]
    public string[]? AddIamRoles { get; set; }

    [CommandSwitch("--remove-iam-roles")]
    public string[]? RemoveIamRoles { get; set; }

    [CommandSwitch("--default-iam-role-arn")]
    public string? DefaultIamRoleArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}