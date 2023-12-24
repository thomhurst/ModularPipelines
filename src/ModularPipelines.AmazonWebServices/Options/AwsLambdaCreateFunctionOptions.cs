using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "create-function")]
public record AwsLambdaCreateFunctionOptions(
[property: CommandSwitch("--function-name")] string FunctionName,
[property: CommandSwitch("--role")] string Role
) : AwsOptions
{
    [CommandSwitch("--runtime")]
    public string? Runtime { get; set; }

    [CommandSwitch("--handler")]
    public string? Handler { get; set; }

    [CommandSwitch("--code")]
    public string? Code { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--timeout")]
    public int? Timeout { get; set; }

    [CommandSwitch("--memory-size")]
    public int? MemorySize { get; set; }

    [CommandSwitch("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CommandSwitch("--package-type")]
    public string? PackageType { get; set; }

    [CommandSwitch("--dead-letter-config")]
    public string? DeadLetterConfig { get; set; }

    [CommandSwitch("--environment")]
    public string? Environment { get; set; }

    [CommandSwitch("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CommandSwitch("--tracing-config")]
    public string? TracingConfig { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--layers")]
    public string[]? Layers { get; set; }

    [CommandSwitch("--file-system-configs")]
    public string[]? FileSystemConfigs { get; set; }

    [CommandSwitch("--image-config")]
    public string? ImageConfig { get; set; }

    [CommandSwitch("--code-signing-config-arn")]
    public string? CodeSigningConfigArn { get; set; }

    [CommandSwitch("--architectures")]
    public string[]? Architectures { get; set; }

    [CommandSwitch("--ephemeral-storage")]
    public string? EphemeralStorage { get; set; }

    [CommandSwitch("--snap-start")]
    public string? SnapStart { get; set; }

    [CommandSwitch("--logging-config")]
    public string? LoggingConfig { get; set; }

    [CommandSwitch("--zip-file")]
    public string? ZipFile { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}