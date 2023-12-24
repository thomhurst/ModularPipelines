using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplify", "start-deployment")]
public record AwsAmplifyStartDeploymentOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--branch-name")] string BranchName
) : AwsOptions
{
    [CommandSwitch("--job-id")]
    public string? JobId { get; set; }

    [CommandSwitch("--source-url")]
    public string? SourceUrl { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}