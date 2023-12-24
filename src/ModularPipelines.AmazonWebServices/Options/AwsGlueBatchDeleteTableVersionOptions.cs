using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "batch-delete-table-version")]
public record AwsGlueBatchDeleteTableVersionOptions(
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--table-name")] string TableName,
[property: CommandSwitch("--version-ids")] string[] VersionIds
) : AwsOptions
{
    [CommandSwitch("--catalog-id")]
    public string? CatalogId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}