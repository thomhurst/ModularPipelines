using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "roles", "create")]
public record GcloudIamRolesCreateOptions : GcloudOptions
{
    public GcloudIamRolesCreateOptions(
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

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--permissions")]
    public string? Permissions { get; set; }

    [CliOption("--stage")]
    public string? Stage { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }
}
