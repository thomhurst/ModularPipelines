using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codepipeline", "get-action-type")]
public record AwsCodepipelineGetActionTypeOptions(
[property: CommandSwitch("--category")] string Category,
[property: CommandSwitch("--owner")] string Owner,
[property: CommandSwitch("--provider")] string Provider,
[property: CommandSwitch("--action-version")] string ActionVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}