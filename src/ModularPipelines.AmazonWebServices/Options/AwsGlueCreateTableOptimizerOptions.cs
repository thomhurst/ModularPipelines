using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "create-table-optimizer")]
public record AwsGlueCreateTableOptimizerOptions(
[property: CliOption("--catalog-id")] string CatalogId,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--table-name")] string TableName,
[property: CliOption("--type")] string Type,
[property: CliOption("--table-optimizer-configuration")] string TableOptimizerConfiguration
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}