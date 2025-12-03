using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("asset", "analyze-iam-policy")]
public record GcloudAssetAnalyzeIamPolicyOptions : GcloudOptions
{
    public GcloudAssetAnalyzeIamPolicyOptions(
        string folder,
        string organization,
        string project
    )
    {
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [CliOption("--folder")]
    public string Folder { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }

    [CliOption("--access-time")]
    public string? AccessTime { get; set; }

    [CliOption("--full-resource-name")]
    public string? FullResourceName { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--saved-analysis-query")]
    public string? SavedAnalysisQuery { get; set; }

    [CliFlag("--analyze-service-account-impersonation")]
    public bool? AnalyzeServiceAccountImpersonation { get; set; }

    [CliOption("--execution-timeout")]
    public string? ExecutionTimeout { get; set; }

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

    [CliFlag("--show-response")]
    public bool? ShowResponse { get; set; }

    [CliOption("--permissions")]
    public string[]? Permissions { get; set; }

    [CliOption("--roles")]
    public string[]? Roles { get; set; }
}
