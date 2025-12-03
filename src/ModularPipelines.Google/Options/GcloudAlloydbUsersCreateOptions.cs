using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "users", "create")]
public record GcloudAlloydbUsersCreateOptions(
[property: CliArgument] string Username,
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--region")] string Region
) : GcloudOptions
{
    [CliOption("--db-roles")]
    public string[]? DbRoles { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--superuser")]
    public string? Superuser { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}