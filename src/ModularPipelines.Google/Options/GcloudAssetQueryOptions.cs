using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("asset", "query")]
public record GcloudAssetQueryOptions : GcloudOptions
{
    public GcloudAssetQueryOptions(
        string folder,
        string organization,
        string project,
        string jobReference,
        string statement
    )
    {
        Folder = folder;
        Organization = organization;
        Project = project;
        JobReference = jobReference;
        Statement = statement;
    }

    [CommandSwitch("--folder")]
    public string Folder { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }

    [CommandSwitch("--job-reference")]
    public string JobReference { get; set; }

    [CommandSwitch("--statement")]
    public string Statement { get; set; }

    [CommandSwitch("--page-size")]
    public string? PageSize { get; set; }

    [CommandSwitch("--page-token")]
    public string? PageToken { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--snapshot-time")]
    public string? SnapshotTime { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }

    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--write-disposition")]
    public string? WriteDisposition { get; set; }

    [BooleanCommandSwitch("write-append")]
    public bool? WriteAppend { get; set; }

    [BooleanCommandSwitch("write-empty")]
    public bool? WriteEmpty { get; set; }

    [BooleanCommandSwitch("write-truncate")]
    public bool? WriteTruncate { get; set; }

    [CommandSwitch("--bigquery-table")]
    public string? BigqueryTable { get; set; }

    [CommandSwitch("--bigquery-dataset")]
    public string? BigqueryDataset { get; set; }
}
