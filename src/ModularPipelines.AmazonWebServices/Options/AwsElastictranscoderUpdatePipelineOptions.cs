using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastictranscoder", "update-pipeline")]
public record AwsElastictranscoderUpdatePipelineOptions(
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--input-bucket")]
    public string? InputBucket { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--aws-kms-key-arn")]
    public string? AwsKmsKeyArn { get; set; }

    [CommandSwitch("--notifications")]
    public string? Notifications { get; set; }

    [CommandSwitch("--content-config")]
    public string? ContentConfig { get; set; }

    [CommandSwitch("--thumbnail-config")]
    public string? ThumbnailConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}