using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("asset", "analyze-iam-policy-longrunning")]
public record GcloudAssetAnalyzeIamPolicyLongrunningOptions : GcloudOptions
{
    public GcloudAssetAnalyzeIamPolicyLongrunningOptions(
        string folder,
        string organization,
        string project,
        string gcsOutputPath,
        string bigqueryDataset,
        string bigqueryTablePrefix,
        string bigqueryPartitionKey,
        string bigqueryWriteDisposition
    )
    {
        Folder = folder;
        Organization = organization;
        Project = project;
        GcsOutputPath = gcsOutputPath;
        BigqueryDataset = bigqueryDataset;
        BigqueryTablePrefix = bigqueryTablePrefix;
        BigqueryPartitionKey = bigqueryPartitionKey;
        BigqueryWriteDisposition = bigqueryWriteDisposition;
    }

    [CommandSwitch("--folder")]
    public string Folder { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }

    [CommandSwitch("--gcs-output-path")]
    public string GcsOutputPath { get; set; }

    [CommandSwitch("--bigquery-dataset")]
    public string BigqueryDataset { get; set; }

    [CommandSwitch("--bigquery-table-prefix")]
    public string BigqueryTablePrefix { get; set; }

    [CommandSwitch("--bigquery-partition-key")]
    public string BigqueryPartitionKey { get; set; }

    [CommandSwitch("--bigquery-write-disposition")]
    public string BigqueryWriteDisposition { get; set; }

    [CommandSwitch("--access-time")]
    public string? AccessTime { get; set; }

    [CommandSwitch("--full-resource-name")]
    public string? FullResourceName { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [BooleanCommandSwitch("--analyze-service-account-impersonation")]
    public bool? AnalyzeServiceAccountImpersonation { get; set; }

    [BooleanCommandSwitch("--expand-groups")]
    public bool? ExpandGroups { get; set; }

    [BooleanCommandSwitch("--expand-resources")]
    public bool? ExpandResources { get; set; }

    [BooleanCommandSwitch("--expand-roles")]
    public bool? ExpandRoles { get; set; }

    [BooleanCommandSwitch("--output-group-edges")]
    public bool? OutputGroupEdges { get; set; }

    [BooleanCommandSwitch("--output-resource-edges")]
    public bool? OutputResourceEdges { get; set; }

    [CommandSwitch("--permissions")]
    public string[]? Permissions { get; set; }

    [CommandSwitch("--roles")]
    public string[]? Roles { get; set; }
}
