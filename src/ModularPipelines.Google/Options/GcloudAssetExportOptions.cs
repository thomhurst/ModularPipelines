using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("asset", "export")]
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

    [CommandSwitch("--folder")]
    public string Folder { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }

    [CommandSwitch("--output-path")]
    public string OutputPath { get; set; }

    [CommandSwitch("--output-path-prefix")]
    public string OutputPathPrefix { get; set; }

    [BooleanCommandSwitch("--output-bigquery-force")]
    public bool OutputBigqueryForce { get; set; }

    [CommandSwitch("--partition-key")]
    public string PartitionKey { get; set; }

    [BooleanCommandSwitch("--per-asset-type")]
    public bool PerAssetType { get; set; }

    [CommandSwitch("--bigquery-table")]
    public string BigqueryTable { get; set; }

    [CommandSwitch("--bigquery-dataset")]
    public string BigqueryDataset { get; set; }

    [CommandSwitch("--asset-types")]
    public string[]? AssetTypes { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--relationship-types")]
    public string[]? RelationshipTypes { get; set; }

    [CommandSwitch("--snapshot-time")]
    public string? SnapshotTime { get; set; }
}
