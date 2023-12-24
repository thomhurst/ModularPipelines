using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codebuild", "update-webhook")]
public record AwsCodebuildUpdateWebhookOptions(
[property: CommandSwitch("--project-name")] string ProjectName
) : AwsOptions
{
    [CommandSwitch("--branch-filter")]
    public string? BranchFilter { get; set; }

    [CommandSwitch("--filter-groups")]
    public string[]? FilterGroups { get; set; }

    [CommandSwitch("--build-type")]
    public string? BuildType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}