using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "create-integration")]
public record AwsRdsCreateIntegrationOptions(
[property: CliOption("--source-arn")] string SourceArn,
[property: CliOption("--target-arn")] string TargetArn,
[property: CliOption("--integration-name")] string IntegrationName
) : AwsOptions
{
    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--additional-encryption-context")]
    public IEnumerable<KeyValue>? AdditionalEncryptionContext { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}