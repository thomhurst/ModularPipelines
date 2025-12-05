using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codebuild", "update-project-visibility")]
public record AwsCodebuildUpdateProjectVisibilityOptions(
[property: CliOption("--project-arn")] string ProjectArn,
[property: CliOption("--project-visibility")] string ProjectVisibility
) : AwsOptions
{
    [CliOption("--resource-access-role")]
    public string? ResourceAccessRole { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}