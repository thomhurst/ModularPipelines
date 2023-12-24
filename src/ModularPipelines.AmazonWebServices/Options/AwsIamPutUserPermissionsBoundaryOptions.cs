using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "put-user-permissions-boundary")]
public record AwsIamPutUserPermissionsBoundaryOptions(
[property: CommandSwitch("--user-name")] string UserName,
[property: CommandSwitch("--permissions-boundary")] string PermissionsBoundary
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}