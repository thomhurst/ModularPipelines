using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplify", "delete-branch")]
public record AwsAmplifyDeleteBranchOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--branch-name")] string BranchName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}