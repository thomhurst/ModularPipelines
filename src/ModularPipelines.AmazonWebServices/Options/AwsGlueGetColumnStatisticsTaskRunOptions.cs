using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "get-column-statistics-task-run")]
public record AwsGlueGetColumnStatisticsTaskRunOptions(
[property: CliOption("--column-statistics-task-run-id")] string ColumnStatisticsTaskRunId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}