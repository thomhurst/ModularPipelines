using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplify", "start-deployment")]
public record AwsAmplifyStartDeploymentOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--branch-name")] string BranchName
) : AwsOptions
{
    [CliOption("--job-id")]
    public string? JobId { get; set; }

    [CliOption("--source-url")]
    public string? SourceUrl { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}