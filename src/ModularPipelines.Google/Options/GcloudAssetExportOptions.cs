using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("asset", "export")]
public record GcloudAssetExportOptions : GcloudOptions
{
    public GcloudAssetExportOptions(
        string folder,
        string organization,
        string project,
        string outputPath,
        string outputPathPrefix,
        bool outputBigqueryForce,
        string partitionKey,
        bool perAssetType,
        string bigqueryTable,
        string bigqueryDataset
    )
    {
        Folder = folder;
        Organization = organization;
        Project = project;
        OutputPath = outputPath;
        OutputPathPrefix = outputPathPrefix;
        OutputBigqueryForce = outputBigqueryForce;
        PartitionKey = partitionKey;
        PerAssetType = perAssetType;
        BigqueryTable = bigqueryTable;
        BigqueryDataset = bigqueryDataset;
    }

    [CliOption("--folder")]
    public string Folder { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }

    [CliOption("--output-path")]
    public string OutputPath { get; set; }

    [CliOption("--output-path-prefix")]
    public string OutputPathPrefix { get; set; }

    [CliFlag("--output-bigquery-force")]
    public bool OutputBigqueryForce { get; set; }

    [CliOption("--partition-key")]
    public string PartitionKey { get; set; }

    [CliFlag("--per-asset-type")]
    public bool PerAssetType { get; set; }

    [CliOption("--bigquery-table")]
    public string BigqueryTable { get; set; }

    [CliOption("--bigquery-dataset")]
    public string BigqueryDataset { get; set; }

    [CliOption("--asset-types")]
    public string[]? AssetTypes { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--relationship-types")]
    public string[]? RelationshipTypes { get; set; }

    [CliOption("--snapshot-time")]
    public string? SnapshotTime { get; set; }
}
