using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisvideo", "create-stream")]
public record AwsKinesisvideoCreateStreamOptions(
[property: CliOption("--stream-name")] string StreamName
) : AwsOptions
{
    [CliOption("--device-name")]
    public string? DeviceName { get; set; }

    [CliOption("--media-type")]
    public string? MediaType { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--data-retention-in-hours")]
    public int? DataRetentionInHours { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}