using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "org-policies", "set-policy")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string PolicyFile { get; set; }

    [CliOption("--folder")]
    public string Folder { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }
}
