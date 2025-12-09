using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "update-function-configuration")]
public record AwsLambdaUpdateFunctionConfigurationOptions(
[property: CliOption("--function-name")] string FunctionName
) : AwsOptions
{
    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--handler")]
    public string? Handler { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--timeout")]
    public int? Timeout { get; set; }

    [CliOption("--memory-size")]
    public int? MemorySize { get; set; }

    [CliOption("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliOption("--runtime")]
    public string? Runtime { get; set; }

    [CliOption("--dead-letter-config")]
    public string? DeadLetterConfig { get; set; }

    [CliOption("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CliOption("--tracing-config")]
    public string? TracingConfig { get; set; }

    [CliOption("--revision-id")]
    public string? RevisionId { get; set; }

    [CliOption("--layers")]
    public string[]? Layers { get; set; }

    [CliOption("--file-system-configs")]
    public string[]? FileSystemConfigs { get; set; }

    [CliOption("--image-config")]
    public string? ImageConfig { get; set; }

    [CliOption("--ephemeral-storage")]
    public string? EphemeralStorage { get; set; }

    [CliOption("--snap-start")]
    public string? SnapStart { get; set; }

    [CliOption("--logging-config")]
    public string? LoggingConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}