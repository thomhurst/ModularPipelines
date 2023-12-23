using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "get-unfiltered-partition-metadata")]
public record AwsGlueGetUnfilteredPartitionMetadataOptions(
[property: CommandSwitch("--catalog-id")] string CatalogId,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--table-name")] string TableName,
[property: CommandSwitch("--partition-values")] string[] PartitionValues,
[property: CommandSwitch("--supported-permission-types")] string[] SupportedPermissionTypes
) : AwsOptions
{
    [CommandSwitch("--audit-context")]
    public string? AuditContext { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}