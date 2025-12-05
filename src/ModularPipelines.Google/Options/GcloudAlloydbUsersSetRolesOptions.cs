using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "users", "set-roles")]
public record GcloudAlloydbUsersSetRolesOptions(
[property: CliArgument] string Username,
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--db-roles")] string[] DbRoles,
[property: CliOption("--region")] string Region
) : GcloudOptions;