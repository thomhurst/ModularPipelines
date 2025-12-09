using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "create-function")]
public record AwsLambdaCreateFunctionOptions(
[property: CliOption("--function-name")] string FunctionName,
[property: CliOption("--role")] string Role
) : AwsOptions
{
    [CliOption("--runtime")]
    public string? Runtime { get; set; }

    [CliOption("--handler")]
    public string? Handler { get; set; }

    [CliOption("--code")]
    public string? Code { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--timeout")]
    public int? Timeout { get; set; }

    [CliOption("--memory-size")]
    public int? MemorySize { get; set; }

    [CliOption("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CliOption("--package-type")]
    public string? PackageType { get; set; }

    [CliOption("--dead-letter-config")]
    public string? DeadLetterConfig { get; set; }

    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliOption("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CliOption("--tracing-config")]
    public string? TracingConfig { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--layers")]
    public string[]? Layers { get; set; }

    [CliOption("--file-system-configs")]
    public string[]? FileSystemConfigs { get; set; }

    [CliOption("--image-config")]
    public string? ImageConfig { get; set; }

    [CliOption("--code-signing-config-arn")]
    public string? CodeSigningConfigArn { get; set; }

    [CliOption("--architectures")]
    public string[]? Architectures { get; set; }

    [CliOption("--ephemeral-storage")]
    public string? EphemeralStorage { get; set; }

    [CliOption("--snap-start")]
    public string? SnapStart { get; set; }

    [CliOption("--logging-config")]
    public string? LoggingConfig { get; set; }

    [CliOption("--zip-file")]
    public string? ZipFile { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}