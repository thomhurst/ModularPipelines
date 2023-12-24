using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "wait", "change-set-create-complete")]
public record AwsCloudformationWaitChangeSetCreateCompleteOptions(
[property: CommandSwitch("--change-set-name")] string ChangeSetName
) : AwsOptions
{
    [CommandSwitch("--stack-name")]
    public string? StackName { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}