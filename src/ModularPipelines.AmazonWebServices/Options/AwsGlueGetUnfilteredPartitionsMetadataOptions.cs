using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "get-unfiltered-partitions-metadata")]
public record AwsGlueGetUnfilteredPartitionsMetadataOptions(
[property: CommandSwitch("--catalog-id")] string CatalogId,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--table-name")] string TableName,
[property: CommandSwitch("--supported-permission-types")] string[] SupportedPermissionTypes
) : AwsOptions
{
    [CommandSwitch("--expression")]
    public string? Expression { get; set; }

    [CommandSwitch("--audit-context")]
    public string? AuditContext { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--segment")]
    public string? Segment { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}