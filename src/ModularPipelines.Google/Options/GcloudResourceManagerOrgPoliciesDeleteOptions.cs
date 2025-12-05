using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "org-policies", "delete")]
public record GcloudResourceManagerOrgPoliciesDeleteOptions : GcloudOptions
{
    public GcloudResourceManagerOrgPoliciesDeleteOptions(
        string orgPolicyId,
        string folder,
        string organization,
        string project
    )
    {
        OrgPolicyId = orgPolicyId;
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string OrgPolicyId { get; set; }

    [CliOption("--folder")]
    public string Folder { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }
}
