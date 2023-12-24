using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "create-integration")]
public record AwsRdsCreateIntegrationOptions(
[property: CommandSwitch("--source-arn")] string SourceArn,
[property: CommandSwitch("--target-arn")] string TargetArn,
[property: CommandSwitch("--integration-name")] string IntegrationName
) : AwsOptions
{
    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--additional-encryption-context")]
    public IEnumerable<KeyValue>? AdditionalEncryptionContext { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}