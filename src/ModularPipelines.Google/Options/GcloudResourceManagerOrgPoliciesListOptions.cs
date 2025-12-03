using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resource-manager", "org-policies", "list")]
public record GcloudResourceManagerOrgPoliciesListOptions : GcloudOptions
{
    public GcloudResourceManagerOrgPoliciesListOptions(
        string folder,
        string organization,
        string project
    )
    {
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [CliOption("--folder")]
    public string Folder { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }

    [CliFlag("--show-unset")]
    public bool? ShowUnset { get; set; }
}
