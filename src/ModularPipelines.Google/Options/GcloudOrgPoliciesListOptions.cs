using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("org-policies", "list")]
public record GcloudOrgPoliciesListOptions : GcloudOptions
{
    public GcloudOrgPoliciesListOptions(
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

    [BooleanCommandSwitch("--show-unset")]
    public bool? ShowUnset { get; set; }
}
