using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "roles", "delete")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string RoleId { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }
}
