using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "update-function-configuration")]
public record AwsLambdaUpdateFunctionConfigurationOptions(
[property: CommandSwitch("--function-name")] string FunctionName
) : AwsOptions
{
    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--handler")]
    public string? Handler { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--timeout")]
    public int? Timeout { get; set; }

    [CommandSwitch("--memory-size")]
    public int? MemorySize { get; set; }

    [CommandSwitch("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CommandSwitch("--environment")]
    public string? Environment { get; set; }

    [CommandSwitch("--runtime")]
    public string? Runtime { get; set; }

    [CommandSwitch("--dead-letter-config")]
    public string? DeadLetterConfig { get; set; }

    [CommandSwitch("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CommandSwitch("--tracing-config")]
    public string? TracingConfig { get; set; }

    [CommandSwitch("--revision-id")]
    public string? RevisionId { get; set; }

    [CommandSwitch("--layers")]
    public string[]? Layers { get; set; }

    [CommandSwitch("--file-system-configs")]
    public string[]? FileSystemConfigs { get; set; }

    [CommandSwitch("--image-config")]
    public string? ImageConfig { get; set; }

    [CommandSwitch("--ephemeral-storage")]
    public string? EphemeralStorage { get; set; }

    [CommandSwitch("--snap-start")]
    public string? SnapStart { get; set; }

    [CommandSwitch("--logging-config")]
    public string? LoggingConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}