using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "org-policies", "deny")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string OrgPolicyId { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string DeniedValue { get; set; }

    [CommandSwitch("--folder")]
    public string Folder { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }
}
