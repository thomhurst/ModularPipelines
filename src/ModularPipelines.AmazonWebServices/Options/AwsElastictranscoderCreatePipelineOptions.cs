using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastictranscoder", "create-pipeline")]
public record AwsElastictranscoderCreatePipelineOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--input-bucket")] string InputBucket,
[property: CommandSwitch("--role")] string Role
) : AwsOptions
{
    [CommandSwitch("--output-bucket")]
    public string? OutputBucket { get; set; }

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