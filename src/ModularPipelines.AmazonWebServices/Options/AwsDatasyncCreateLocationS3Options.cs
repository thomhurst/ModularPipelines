using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "create-location-s3")]
public record AwsDatasyncCreateLocationS3Options(
[property: CommandSwitch("--s3-bucket-arn")] string S3BucketArn,
[property: CommandSwitch("--s3-config")] string S3Config
) : AwsOptions
{
    [CommandSwitch("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CommandSwitch("--s3-storage-class")]
    public string? S3StorageClass { get; set; }

    [CommandSwitch("--agent-arns")]
    public string[]? AgentArns { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}