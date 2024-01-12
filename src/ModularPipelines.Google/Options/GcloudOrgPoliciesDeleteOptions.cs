using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("org-policies", "delete")]
public record GcloudOrgPoliciesDeleteOptions : GcloudOptions
{
    public GcloudOrgPoliciesDeleteOptions(
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

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }
}
