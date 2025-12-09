using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("discovery", "start-export-task")]
public record AwsDiscoveryStartExportTaskOptions : AwsOptions
{
    [CliOption("--export-data-format")]
    public string[]? ExportDataFormat { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--preferences")]
    public string? Preferences { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}