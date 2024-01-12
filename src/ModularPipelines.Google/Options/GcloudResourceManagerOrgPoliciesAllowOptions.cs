using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "org-policies", "allow")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string OrgPolicyId { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string AllowedValue { get; set; }

    [CommandSwitch("--folder")]
    public string Folder { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }
}
