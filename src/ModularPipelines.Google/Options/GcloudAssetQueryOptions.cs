using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("asset", "query")]
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

    [CliOption("--folder")]
    public string Folder { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }

    [CliOption("--job-reference")]
    public string JobReference { get; set; }

    [CliOption("--statement")]
    public string Statement { get; set; }

    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--page-token")]
    public string? PageToken { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--snapshot-time")]
    public string? SnapshotTime { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--write-disposition")]
    public string? WriteDisposition { get; set; }

    [CliFlag("write-append")]
    public bool? WriteAppend { get; set; }

    [CliFlag("write-empty")]
    public bool? WriteEmpty { get; set; }

    [CliFlag("write-truncate")]
    public bool? WriteTruncate { get; set; }

    [CliOption("--bigquery-table")]
    public string? BigqueryTable { get; set; }

    [CliOption("--bigquery-dataset")]
    public string? BigqueryDataset { get; set; }
}
