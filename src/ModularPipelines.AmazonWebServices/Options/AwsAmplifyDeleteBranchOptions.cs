using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplify", "delete-branch")]
public record AwsAmplifyDeleteBranchOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--branch-name")] string BranchName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}