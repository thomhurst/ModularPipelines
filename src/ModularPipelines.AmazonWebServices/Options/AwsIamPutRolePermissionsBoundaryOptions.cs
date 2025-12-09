using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "put-role-permissions-boundary")]
public record AwsIamPutRolePermissionsBoundaryOptions(
[property: CliOption("--role-name")] string RoleName,
[property: CliOption("--permissions-boundary")] string PermissionsBoundary
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}