using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "roles", "delete")]
public record GcloudIamRolesDeleteOptions : GcloudOptions
{
    public GcloudIamRolesDeleteOptions(
        string roleId,
        string organization,
        string project
    )
    {
        RoleId = roleId;
        Organization = organization;
        Project = project;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string RoleId { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }
}
