using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplify", "update-app")]
public record AwsAmplifyUpdateAppOptions(
[property: CommandSwitch("--app-id")] string AppId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--iam-service-role-arn")]
    public string? IamServiceRoleArn { get; set; }

    [CommandSwitch("--environment-variables")]
    public IEnumerable<KeyValue>? AwsAmplEnvironmentVariables { get; set; }

    [CommandSwitch("--basic-auth-credentials")]
    public string? BasicAuthCredentials { get; set; }

    [CommandSwitch("--custom-rules")]
    public string[]? CustomRules { get; set; }

    [CommandSwitch("--build-spec")]
    public string? BuildSpec { get; set; }

    [CommandSwitch("--custom-headers")]
    public string? CustomHeaders { get; set; }

    [CommandSwitch("--auto-branch-creation-patterns")]
    public string[]? AutoBranchCreationPatterns { get; set; }

    [CommandSwitch("--auto-branch-creation-config")]
    public string? AutoBranchCreationConfig { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }

    [CommandSwitch("--oauth-token")]
    public string? OauthToken { get; set; }

    [CommandSwitch("--access-token")]
    public string? AccessToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}