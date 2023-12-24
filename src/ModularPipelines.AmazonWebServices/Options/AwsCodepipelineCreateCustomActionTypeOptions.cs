using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "create-custom-action-type")]
public record AwsCodepipelineCreateCustomActionTypeOptions(
[property: CommandSwitch("--category")] string Category,
[property: CommandSwitch("--provider")] string Provider,
[property: CommandSwitch("--input-artifact-details")] string InputArtifactDetails,
[property: CommandSwitch("--output-artifact-details")] string OutputArtifactDetails,
[property: CommandSwitch("--action-version")] string ActionVersion
) : AwsOptions
{
    [CommandSwitch("--settings")]
    public string? Settings { get; set; }

    [CommandSwitch("--configuration-properties")]
    public string[]? ConfigurationProperties { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}