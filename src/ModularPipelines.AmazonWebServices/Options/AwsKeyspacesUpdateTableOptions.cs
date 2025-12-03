using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyspaces", "update-table")]
public record AwsKeyspacesUpdateTableOptions(
[property: CliOption("--keyspace-name")] string KeyspaceName,
[property: CliOption("--table-name")] string TableName
) : AwsOptions
{
    [CliOption("--add-columns")]
    public string[]? AddColumns { get; set; }

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

    [CliOption("--client-side-timestamps")]
    public string? ClientSideTimestamps { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}