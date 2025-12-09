using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "create-stream-processor")]
public record AwsRekognitionCreateStreamProcessorOptions(
[property: CliOption("--input")] string Input,
[property: CliOption("--name")] string Name,
[property: CliOption("--settings")] string Settings,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--stream-processor-output")] string StreamProcessorOutput
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--notification-channel")]
    public string? NotificationChannel { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--regions-of-interest")]
    public string[]? RegionsOfInterest { get; set; }

    [CliOption("--data-sharing-preference")]
    public string? DataSharingPreference { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}