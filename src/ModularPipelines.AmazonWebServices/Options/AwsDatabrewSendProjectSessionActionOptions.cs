using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("databrew", "send-project-session-action")]
public record AwsDatabrewSendProjectSessionActionOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--recipe-step")]
    public string? RecipeStep { get; set; }

    [CliOption("--step-index")]
    public int? StepIndex { get; set; }

    [CliOption("--client-session-id")]
    public string? ClientSessionId { get; set; }

    [CliOption("--view-frame")]
    public string? ViewFrame { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}