using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "create-role")]
public record AwsIamCreateRoleOptions(
[property: CommandSwitch("--role-name")] string RoleName,
[property: CommandSwitch("--assume-role-policy-document")] string AssumeRolePolicyDocument
) : AwsOptions
{
    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--max-session-duration")]
    public int? MaxSessionDuration { get; set; }

    [CommandSwitch("--permissions-boundary")]
    public string? PermissionsBoundary { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}