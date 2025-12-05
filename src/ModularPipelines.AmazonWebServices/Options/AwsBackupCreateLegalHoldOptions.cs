using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "create-legal-hold")]
public record AwsBackupCreateLegalHoldOptions(
[property: CliOption("--title")] string Title,
[property: CliOption("--description")] string Description
) : AwsOptions
{
    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--recovery-point-selection")]
    public string? RecoveryPointSelection { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}