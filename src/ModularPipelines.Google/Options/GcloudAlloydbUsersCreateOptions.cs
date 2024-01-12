using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alloydb", "users", "create")]
public record GcloudAlloydbUsersCreateOptions(
[property: PositionalArgument] string Username,
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions
{
    [CommandSwitch("--db-roles")]
    public string[]? DbRoles { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--superuser")]
    public string? Superuser { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}