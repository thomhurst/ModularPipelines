using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "package")]
public record AwsCloudformationPackageOptions(
[property: CommandSwitch("--template-file")] string TemplateFile,
[property: CommandSwitch("--s3-bucket")] string S3Bucket
) : AwsOptions
{
    [CommandSwitch("--s3-prefix")]
    public string? S3Prefix { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--output-template-file")]
    public string? OutputTemplateFile { get; set; }

    [BooleanCommandSwitch("--use-json")]
    public bool? UseJson { get; set; }

    [BooleanCommandSwitch("--force-upload")]
    public bool? ForceUpload { get; set; }

    [CommandSwitch("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }
}