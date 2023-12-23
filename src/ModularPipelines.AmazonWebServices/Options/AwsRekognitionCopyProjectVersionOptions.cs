using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "copy-project-version")]
public record AwsRekognitionCopyProjectVersionOptions(
[property: CommandSwitch("--source-project-arn")] string SourceProjectArn,
[property: CommandSwitch("--source-project-version-arn")] string SourceProjectVersionArn,
[property: CommandSwitch("--destination-project-arn")] string DestinationProjectArn,
[property: CommandSwitch("--version-name")] string VersionName,
[property: CommandSwitch("--output-config")] string OutputConfig
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}