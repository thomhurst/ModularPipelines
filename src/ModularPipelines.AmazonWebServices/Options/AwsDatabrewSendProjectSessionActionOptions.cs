using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("databrew", "send-project-session-action")]
public record AwsDatabrewSendProjectSessionActionOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--recipe-step")]
    public string? RecipeStep { get; set; }

    [CommandSwitch("--step-index")]
    public int? StepIndex { get; set; }

    [CommandSwitch("--client-session-id")]
    public string? ClientSessionId { get; set; }

    [CommandSwitch("--view-frame")]
    public string? ViewFrame { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}