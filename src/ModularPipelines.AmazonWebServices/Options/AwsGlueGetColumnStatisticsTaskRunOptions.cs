using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "get-column-statistics-task-run")]
public record AwsGlueGetColumnStatisticsTaskRunOptions(
[property: CommandSwitch("--column-statistics-task-run-id")] string ColumnStatisticsTaskRunId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}