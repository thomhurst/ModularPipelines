using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisvideo", "create-stream")]
public record AwsKinesisvideoCreateStreamOptions(
[property: CommandSwitch("--stream-name")] string StreamName
) : AwsOptions
{
    [CommandSwitch("--device-name")]
    public string? DeviceName { get; set; }

    [CommandSwitch("--media-type")]
    public string? MediaType { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--data-retention-in-hours")]
    public int? DataRetentionInHours { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}