using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "get-action-type")]
public record AwsCodepipelineGetActionTypeOptions(
[property: CliOption("--category")] string Category,
[property: CliOption("--owner")] string Owner,
[property: CliOption("--provider")] string Provider,
[property: CliOption("--action-version")] string ActionVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}