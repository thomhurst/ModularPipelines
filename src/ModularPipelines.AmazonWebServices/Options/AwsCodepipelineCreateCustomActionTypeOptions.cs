using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codepipeline", "create-custom-action-type")]
public record AwsCodepipelineCreateCustomActionTypeOptions(
[property: CliOption("--category")] string Category,
[property: CliOption("--provider")] string Provider,
[property: CliOption("--input-artifact-details")] string InputArtifactDetails,
[property: CliOption("--output-artifact-details")] string OutputArtifactDetails,
[property: CliOption("--action-version")] string ActionVersion
) : AwsOptions
{
    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--configuration-properties")]
    public string[]? ConfigurationProperties { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}