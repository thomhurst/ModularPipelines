using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("athena", "get-database")]
public record AwsAthenaGetDatabaseOptions(
[property: CliOption("--catalog-name")] string CatalogName,
[property: CliOption("--database-name")] string DatabaseName
) : AwsOptions
{
    [CliOption("--work-group")]
    public string? WorkGroup { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}