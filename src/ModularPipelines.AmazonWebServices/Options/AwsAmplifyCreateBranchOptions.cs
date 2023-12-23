using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplify", "create-branch")]
public record AwsAmplifyCreateBranchOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--branch-name")] string BranchName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--stage")]
    public string? Stage { get; set; }

    [CommandSwitch("--framework")]
    public string? Framework { get; set; }

    [CommandSwitch("--environment-variables")]
    public IEnumerable<KeyValue>? AwsAmplEnvironmentVariables { get; set; }

    [CommandSwitch("--basic-auth-credentials")]
    public string? BasicAuthCredentials { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--build-spec")]
    public string? BuildSpec { get; set; }

    [CommandSwitch("--ttl")]
    public string? Ttl { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--pull-request-environment-name")]
    public string? PullRequestEnvironmentName { get; set; }

    [CommandSwitch("--backend-environment-arn")]
    public string? BackendEnvironmentArn { get; set; }

    [CommandSwitch("--backend")]
    public string? Backend { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}