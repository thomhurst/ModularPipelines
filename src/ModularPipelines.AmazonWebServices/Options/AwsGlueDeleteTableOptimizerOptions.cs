using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "delete-table-optimizer")]
public record AwsGlueDeleteTableOptimizerOptions(
[property: CliOption("--catalog-id")] string CatalogId,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--table-name")] string TableName,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}