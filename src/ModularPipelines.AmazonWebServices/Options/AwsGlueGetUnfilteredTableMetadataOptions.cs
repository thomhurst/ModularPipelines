using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "get-unfiltered-table-metadata")]
public record AwsGlueGetUnfilteredTableMetadataOptions(
[property: CliOption("--catalog-id")] string CatalogId,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--name")] string Name,
[property: CliOption("--supported-permission-types")] string[] SupportedPermissionTypes
) : AwsOptions
{
    [CliOption("--audit-context")]
    public string? AuditContext { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}