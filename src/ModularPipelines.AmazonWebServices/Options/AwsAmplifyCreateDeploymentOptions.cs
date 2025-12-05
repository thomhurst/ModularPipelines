using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplify", "create-deployment")]
public record AwsAmplifyCreateDeploymentOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--branch-name")] string BranchName
) : AwsOptions
{
    [CliOption("--file-map")]
    public IEnumerable<KeyValue>? FileMap { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}