using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alloydb", "users", "set-superuser")]
public record GcloudAlloydbUsersSetSuperuserOptions(
[property: PositionalArgument] string Username,
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--region")] string Region,
[property: CommandSwitch("--superuser")] string Superuser
) : GcloudOptions;