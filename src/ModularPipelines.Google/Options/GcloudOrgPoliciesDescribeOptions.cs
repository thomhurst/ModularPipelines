using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("org-policies", "describe")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Constraint { get; set; }

    [CommandSwitch("--folder")]
    public string Folder { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }

    [BooleanCommandSwitch("--effective")]
    public bool? Effective { get; set; }
}
