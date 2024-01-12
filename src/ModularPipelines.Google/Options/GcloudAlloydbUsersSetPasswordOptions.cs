using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alloydb", "users", "set-password")]
public record GcloudAlloydbUsersSetPasswordOptions(
[property: PositionalArgument] string Username,
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--password")] string Password,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;