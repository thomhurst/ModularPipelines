using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "org-policies", "allow")]
public record GcloudResourceManagerOrgPoliciesAllowOptions : GcloudOptions
{
    public GcloudResourceManagerOrgPoliciesAllowOptions(
        string orgPolicyId,
        string allowedValue,
        string folder,
        string organization,
        string project
    )
    {
        OrgPolicyId = orgPolicyId;
        AllowedValue = allowedValue;
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string OrgPolicyId { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string AllowedValue { get; set; }

    [CliOption("--folder")]
    public string Folder { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }
}
