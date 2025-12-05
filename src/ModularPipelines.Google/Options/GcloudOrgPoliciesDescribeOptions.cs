using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("org-policies", "describe")]
public record GcloudOrgPoliciesDescribeOptions : GcloudOptions
{
    public GcloudOrgPoliciesDescribeOptions(
        string constraint,
        string folder,
        string organization,
        string project
    )
    {
        Constraint = constraint;
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Constraint { get; set; }

    [CliOption("--folder")]
    public string Folder { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }

    [CliFlag("--effective")]
    public bool? Effective { get; set; }
}
