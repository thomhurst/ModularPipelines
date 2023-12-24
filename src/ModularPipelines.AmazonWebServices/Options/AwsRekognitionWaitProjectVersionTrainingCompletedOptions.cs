using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "wait", "project-version-training-completed")]
public record AwsRekognitionWaitProjectVersionTrainingCompletedOptions(
[property: CommandSwitch("--project-arn")] string ProjectArn
) : AwsOptions
{
    [CommandSwitch("--version-names")]
    public string[]? VersionNames { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}