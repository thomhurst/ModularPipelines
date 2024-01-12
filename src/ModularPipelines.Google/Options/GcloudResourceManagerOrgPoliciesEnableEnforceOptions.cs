using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "org-policies", "enable-enforce")]
public record GcloudResourceManagerOrgPoliciesEnableEnforceOptions : GcloudOptions
{
    public GcloudResourceManagerOrgPoliciesEnableEnforceOptions(
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string OrgPolicyId { get; set; }

    [CommandSwitch("--folder")]
    public string Folder { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }
}
