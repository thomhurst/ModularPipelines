using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "get-table-metadata")]
public record AwsAthenaGetTableMetadataOptions(
[property: CliOption("--catalog-name")] string CatalogName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--table-name")] string TableName
) : AwsOptions
{
    [CliOption("--work-group")]
    public string? WorkGroup { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}