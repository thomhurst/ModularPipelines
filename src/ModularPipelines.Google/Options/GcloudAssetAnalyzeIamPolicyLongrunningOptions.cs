using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("asset", "analyze-iam-policy-longrunning")]
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

    [CliOption("--folder")]
    public string Folder { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }

    [CliOption("--gcs-output-path")]
    public string GcsOutputPath { get; set; }

    [CliOption("--bigquery-dataset")]
    public string BigqueryDataset { get; set; }

    [CliOption("--bigquery-table-prefix")]
    public string BigqueryTablePrefix { get; set; }

    [CliOption("--bigquery-partition-key")]
    public string BigqueryPartitionKey { get; set; }

    [CliOption("--bigquery-write-disposition")]
    public string BigqueryWriteDisposition { get; set; }

    [CliOption("--access-time")]
    public string? AccessTime { get; set; }

    [CliOption("--full-resource-name")]
    public string? FullResourceName { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliFlag("--analyze-service-account-impersonation")]
    public bool? AnalyzeServiceAccountImpersonation { get; set; }

    [CliFlag("--expand-groups")]
    public bool? ExpandGroups { get; set; }

    [CliFlag("--expand-resources")]
    public bool? ExpandResources { get; set; }

    [CliFlag("--expand-roles")]
    public bool? ExpandRoles { get; set; }

    [CliFlag("--output-group-edges")]
    public bool? OutputGroupEdges { get; set; }

    [CliFlag("--output-resource-edges")]
    public bool? OutputResourceEdges { get; set; }

    [CliOption("--permissions")]
    public string[]? Permissions { get; set; }

    [CliOption("--roles")]
    public string[]? Roles { get; set; }
}
