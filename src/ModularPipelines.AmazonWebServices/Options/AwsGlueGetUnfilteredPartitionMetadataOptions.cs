using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "get-unfiltered-partition-metadata")]
public record AwsGlueGetUnfilteredPartitionMetadataOptions(
[property: CliOption("--catalog-id")] string CatalogId,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--table-name")] string TableName,
[property: CliOption("--partition-values")] string[] PartitionValues,
[property: CliOption("--supported-permission-types")] string[] SupportedPermissionTypes
) : AwsOptions
{
    [CliOption("--audit-context")]
    public string? AuditContext { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}