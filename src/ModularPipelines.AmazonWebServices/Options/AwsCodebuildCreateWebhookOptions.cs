using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codebuild", "create-webhook")]
public record AwsCodebuildCreateWebhookOptions(
[property: CliOption("--project-name")] string ProjectName
) : AwsOptions
{
    [CliOption("--branch-filter")]
    public string? BranchFilter { get; set; }

    [CliOption("--filter-groups")]
    public string[]? FilterGroups { get; set; }

    [CliOption("--build-type")]
    public string? BuildType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}