using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "update-stream-processor")]
public record AwsRekognitionUpdateStreamProcessorOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--settings-for-update")]
    public string? SettingsForUpdate { get; set; }

    [CliOption("--regions-of-interest-for-update")]
    public string[]? RegionsOfInterestForUpdate { get; set; }

    [CliOption("--data-sharing-preference-for-update")]
    public string? DataSharingPreferenceForUpdate { get; set; }

    [CliOption("--parameters-to-delete")]
    public string[]? ParametersToDelete { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}