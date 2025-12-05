using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplify", "update-branch")]
public record AwsAmplifyUpdateBranchOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--branch-name")] string BranchName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--framework")]
    public string? Framework { get; set; }

    [CliOption("--stage")]
    public string? Stage { get; set; }

    [CliOption("--environment-variables")]
    public IEnumerable<KeyValue>? AwsAmplEnvironmentVariables { get; set; }

    [CliOption("--basic-auth-credentials")]
    public string? BasicAuthCredentials { get; set; }

    [CliOption("--build-spec")]
    public string? BuildSpec { get; set; }

    [CliOption("--ttl")]
    public string? Ttl { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--pull-request-environment-name")]
    public string? PullRequestEnvironmentName { get; set; }

    [CliOption("--backend-environment-arn")]
    public string? BackendEnvironmentArn { get; set; }

    [CliOption("--backend")]
    public string? Backend { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}