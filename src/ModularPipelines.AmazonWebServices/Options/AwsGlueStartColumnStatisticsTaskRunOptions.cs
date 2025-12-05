using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "start-column-statistics-task-run")]
public record AwsGlueStartColumnStatisticsTaskRunOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--table-name")] string TableName,
[property: CliOption("--role")] string Role
) : AwsOptions
{
    [CliOption("--column-name-list")]
    public string[]? ColumnNameList { get; set; }

    [CliOption("--sample-size")]
    public double? SampleSize { get; set; }

    [CliOption("--catalog-id")]
    public string? CatalogId { get; set; }

    [CliOption("--security-configuration")]
    public string? SecurityConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}