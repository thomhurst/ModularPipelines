using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("postgres", "flexible-server", "connect")]
public record AzPostgresFlexibleServerConnectOptions(
[property: CommandSwitch("--admin-user")] string AdminUser,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--admin-password")]
    public string? AdminPassword { get; set; }

    [CommandSwitch("--database-name")]
    public string? DatabaseName { get; set; }

    [CommandSwitch("--interactive")]
    public string? Interactive { get; set; }

    [CommandSwitch("--querytext")]
    public string? Querytext { get; set; }
}