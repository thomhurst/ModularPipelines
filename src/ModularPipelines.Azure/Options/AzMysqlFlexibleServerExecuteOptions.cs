using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mysql", "flexible-server", "execute")]
public record AzMysqlFlexibleServerExecuteOptions(
[property: CommandSwitch("--admin-password")] string AdminPassword,
[property: CommandSwitch("--admin-user")] string AdminUser,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--database-name")]
    public string? DatabaseName { get; set; }

    [CommandSwitch("--file-path")]
    public string? FilePath { get; set; }

    [CommandSwitch("--querytext")]
    public string? Querytext { get; set; }
}

