using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplify", "get-job")]
public record AwsAmplifyGetJobOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--branch-name")] string BranchName,
[property: CliOption("--job-id")] string JobId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}