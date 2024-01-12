using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("asset", "analyze-iam-policy")]
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

    [CommandSwitch("--folder")]
    public string Folder { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }

    [CommandSwitch("--access-time")]
    public string? AccessTime { get; set; }

    [CommandSwitch("--full-resource-name")]
    public string? FullResourceName { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--saved-analysis-query")]
    public string? SavedAnalysisQuery { get; set; }

    [BooleanCommandSwitch("--analyze-service-account-impersonation")]
    public bool? AnalyzeServiceAccountImpersonation { get; set; }

    [CommandSwitch("--execution-timeout")]
    public string? ExecutionTimeout { get; set; }

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

    [BooleanCommandSwitch("--show-response")]
    public bool? ShowResponse { get; set; }

    [CommandSwitch("--permissions")]
    public string[]? Permissions { get; set; }

    [CommandSwitch("--roles")]
    public string[]? Roles { get; set; }
}
