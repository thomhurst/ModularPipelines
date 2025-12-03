using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplify", "update-app")]
public record AwsAmplifyUpdateAppOptions(
[property: CliOption("--app-id")] string AppId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--platform")]
    public string? Platform { get; set; }

    [CliOption("--iam-service-role-arn")]
    public string? IamServiceRoleArn { get; set; }

    [CliOption("--environment-variables")]
    public IEnumerable<KeyValue>? AwsAmplEnvironmentVariables { get; set; }

    [CliOption("--basic-auth-credentials")]
    public string? BasicAuthCredentials { get; set; }

    [CliOption("--custom-rules")]
    public string[]? CustomRules { get; set; }

    [CliOption("--build-spec")]
    public string? BuildSpec { get; set; }

    [CliOption("--custom-headers")]
    public string? CustomHeaders { get; set; }

    [CliOption("--auto-branch-creation-patterns")]
    public string[]? AutoBranchCreationPatterns { get; set; }

    [CliOption("--auto-branch-creation-config")]
    public string? AutoBranchCreationConfig { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }

    [CliOption("--oauth-token")]
    public string? OauthToken { get; set; }

    [CliOption("--access-token")]
    public string? AccessToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}