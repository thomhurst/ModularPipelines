using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "update-stream-processor")]
public record AwsRekognitionUpdateStreamProcessorOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--settings-for-update")]
    public string? SettingsForUpdate { get; set; }

    [CommandSwitch("--regions-of-interest-for-update")]
    public string[]? RegionsOfInterestForUpdate { get; set; }

    [CommandSwitch("--data-sharing-preference-for-update")]
    public string? DataSharingPreferenceForUpdate { get; set; }

    [CommandSwitch("--parameters-to-delete")]
    public string[]? ParametersToDelete { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}