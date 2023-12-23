using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "create-stream-processor")]
public record AwsRekognitionCreateStreamProcessorOptions(
[property: CommandSwitch("--input")] string Input,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--settings")] string Settings,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--stream-processor-output")] string StreamProcessorOutput
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--notification-channel")]
    public string? NotificationChannel { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--regions-of-interest")]
    public string[]? RegionsOfInterest { get; set; }

    [CommandSwitch("--data-sharing-preference")]
    public string? DataSharingPreference { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}