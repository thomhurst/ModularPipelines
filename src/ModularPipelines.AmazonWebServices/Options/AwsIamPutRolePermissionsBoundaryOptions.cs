using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "put-role-permissions-boundary")]
public record AwsIamPutRolePermissionsBoundaryOptions(
[property: CommandSwitch("--role-name")] string RoleName,
[property: CommandSwitch("--permissions-boundary")] string PermissionsBoundary
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}