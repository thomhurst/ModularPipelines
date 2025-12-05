using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "create-role")]
public record AwsIamCreateRoleOptions(
[property: CliOption("--role-name")] string RoleName,
[property: CliOption("--assume-role-policy-document")] string AssumeRolePolicyDocument
) : AwsOptions
{
    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--max-session-duration")]
    public int? MaxSessionDuration { get; set; }

    [CliOption("--permissions-boundary")]
    public string? PermissionsBoundary { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}