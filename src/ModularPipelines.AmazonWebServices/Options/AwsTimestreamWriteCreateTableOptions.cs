using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("timestream-write", "create-table")]
public record AwsTimestreamWriteCreateTableOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--table-name")] string TableName
) : AwsOptions
{
    [CliOption("--retention-properties")]
    public string? RetentionProperties { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--magnetic-store-write-properties")]
    public string? MagneticStoreWriteProperties { get; set; }

    [CliOption("--schema")]
    public string? Schema { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}