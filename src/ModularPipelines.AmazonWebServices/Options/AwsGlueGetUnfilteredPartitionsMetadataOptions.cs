using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "get-unfiltered-partitions-metadata")]
public record AwsGlueGetUnfilteredPartitionsMetadataOptions(
[property: CliOption("--catalog-id")] string CatalogId,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--table-name")] string TableName,
[property: CliOption("--supported-permission-types")] string[] SupportedPermissionTypes
) : AwsOptions
{
    [CliOption("--expression")]
    public string? Expression { get; set; }

    [CliOption("--audit-context")]
    public string? AuditContext { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--segment")]
    public string? Segment { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}