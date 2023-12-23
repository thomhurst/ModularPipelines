using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "start-column-statistics-task-run")]
public record AwsGlueStartColumnStatisticsTaskRunOptions(
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--table-name")] string TableName,
[property: CommandSwitch("--role")] string Role
) : AwsOptions
{
    [CommandSwitch("--column-name-list")]
    public string[]? ColumnNameList { get; set; }

    [CommandSwitch("--sample-size")]
    public double? SampleSize { get; set; }

    [CommandSwitch("--catalog-id")]
    public string? CatalogId { get; set; }

    [CommandSwitch("--security-configuration")]
    public string? SecurityConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}