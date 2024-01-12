using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alloydb", "users", "set-roles")]
public record GcloudAlloydbUsersSetRolesOptions(
[property: PositionalArgument] string Username,
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--db-roles")] string[] DbRoles,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;