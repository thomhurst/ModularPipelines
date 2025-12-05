using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "package")]
public record AwsCloudformationPackageOptions(
[property: CliOption("--template-file")] string TemplateFile,
[property: CliOption("--s3-bucket")] string S3Bucket
) : AwsOptions
{
    [CliOption("--s3-prefix")]
    public string? S3Prefix { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--output-template-file")]
    public string? OutputTemplateFile { get; set; }

    [CliFlag("--use-json")]
    public bool? UseJson { get; set; }

    [CliFlag("--force-upload")]
    public bool? ForceUpload { get; set; }

    [CliOption("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }
}