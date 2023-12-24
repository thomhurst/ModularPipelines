using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codebuild", "update-project-visibility")]
public record AwsCodebuildUpdateProjectVisibilityOptions(
[property: CommandSwitch("--project-arn")] string ProjectArn,
[property: CommandSwitch("--project-visibility")] string ProjectVisibility
) : AwsOptions
{
    [CommandSwitch("--resource-access-role")]
    public string? ResourceAccessRole { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}