using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "delete-custom-action-type")]
public record AwsCodepipelineDeleteCustomActionTypeOptions(
[property: CommandSwitch("--category")] string Category,
[property: CommandSwitch("--provider")] string Provider,
[property: CommandSwitch("--action-version")] string ActionVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}