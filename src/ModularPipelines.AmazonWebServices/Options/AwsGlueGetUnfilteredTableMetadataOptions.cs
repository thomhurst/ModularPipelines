using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "get-unfiltered-table-metadata")]
public record AwsGlueGetUnfilteredTableMetadataOptions(
[property: CommandSwitch("--catalog-id")] string CatalogId,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--supported-permission-types")] string[] SupportedPermissionTypes
) : AwsOptions
{
    [CommandSwitch("--audit-context")]
    public string? AuditContext { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}