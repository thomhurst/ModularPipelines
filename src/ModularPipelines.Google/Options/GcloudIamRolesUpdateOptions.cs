using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "roles", "update")]
public record GcloudIamRolesUpdateOptions : GcloudOptions
{
    public GcloudIamRolesUpdateOptions(
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

    [CliOption("--add-permissions")]
    public string? AddPermissions { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--permissions")]
    public string? Permissions { get; set; }

    [CliOption("--remove-permissions")]
    public string? RemovePermissions { get; set; }

    [CliOption("--stage")]
    public string? Stage { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }
}
