using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "update-function-code")]
public record AwsLambdaUpdateFunctionCodeOptions(
[property: CommandSwitch("--function-name")] string FunctionName
) : AwsOptions
{
    [CommandSwitch("--zip-file")]
    public string? ZipFile { get; set; }

    [CommandSwitch("--s3-bucket")]
    public string? S3Bucket { get; set; }

    [CommandSwitch("--s3-key")]
    public string? S3Key { get; set; }

    [CommandSwitch("--s3-object-version")]
    public string? S3ObjectVersion { get; set; }

    [CommandSwitch("--image-uri")]
    public string? ImageUri { get; set; }

    [CommandSwitch("--revision-id")]
    public string? RevisionId { get; set; }

    [CommandSwitch("--architectures")]
    public string[]? Architectures { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}