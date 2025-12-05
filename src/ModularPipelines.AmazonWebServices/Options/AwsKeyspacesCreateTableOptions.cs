using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyspaces", "create-table")]
public record AwsKeyspacesCreateTableOptions(
[property: CliOption("--keyspace-name")] string KeyspaceName,
[property: CliOption("--table-name")] string TableName,
[property: CliOption("--schema-definition")] string SchemaDefinition
) : AwsOptions
{
    [CliOption("--comment")]
    public string? Comment { get; set; }

    [CliOption("--capacity-specification")]
    public string? CapacitySpecification { get; set; }

    [CliOption("--encryption-specification")]
    public string? EncryptionSpecification { get; set; }

    [CliOption("--point-in-time-recovery")]
    public string? PointInTimeRecovery { get; set; }

    [CliOption("--ttl")]
    public string? Ttl { get; set; }

    [CliOption("--default-time-to-live")]
    public int? DefaultTimeToLive { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-side-timestamps")]
    public string? ClientSideTimestamps { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}