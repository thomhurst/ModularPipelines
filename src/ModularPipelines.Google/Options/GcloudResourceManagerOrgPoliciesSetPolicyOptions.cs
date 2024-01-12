using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resource-manager", "org-policies", "set-policy")]
public record GcloudResourceManagerOrgPoliciesSetPolicyOptions : GcloudOptions
{
    public GcloudResourceManagerOrgPoliciesSetPolicyOptions(
        string policyFile,
        string folder,
        string organization,
        string project
    )
    {
        PolicyFile = policyFile;
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string PolicyFile { get; set; }

    [CommandSwitch("--folder")]
    public string Folder { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }
}
