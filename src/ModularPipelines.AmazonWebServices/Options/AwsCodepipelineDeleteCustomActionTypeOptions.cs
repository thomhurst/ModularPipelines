using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "delete-custom-action-type")]
public record AwsCodepipelineDeleteCustomActionTypeOptions(
[property: CliOption("--category")] string Category,
[property: CliOption("--provider")] string Provider,
[property: CliOption("--action-version")] string ActionVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}