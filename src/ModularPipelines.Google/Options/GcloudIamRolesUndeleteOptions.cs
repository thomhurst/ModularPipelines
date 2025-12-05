using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "roles", "undelete")]
public record GcloudIamRolesUndeleteOptions : GcloudOptions
{
    public GcloudIamRolesUndeleteOptions(
        string roleId,
        string organization,
        string project
    )
    {
        RoleId = roleId;
        Organization = organization;
        Project = project;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string RoleId { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }
}
