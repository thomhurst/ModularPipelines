using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "org-policies", "deny")]
public record GcloudResourceManagerOrgPoliciesDenyOptions : GcloudOptions
{
    public GcloudResourceManagerOrgPoliciesDenyOptions(
        string orgPolicyId,
        string deniedValue,
        string folder,
        string organization,
        string project
    )
    {
        OrgPolicyId = orgPolicyId;
        DeniedValue = deniedValue;
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string OrgPolicyId { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string DeniedValue { get; set; }

    [CliOption("--folder")]
    public string Folder { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }
}
