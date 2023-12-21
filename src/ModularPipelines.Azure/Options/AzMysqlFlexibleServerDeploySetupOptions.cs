using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mysql", "flexible-server", "deploy", "setup")]
public record AzMysqlFlexibleServerDeploySetupOptions(
[property: CommandSwitch("--admin-password")] string AdminPassword,
[property: CommandSwitch("--admin-user")] string AdminUser,
[property: CommandSwitch("--repo")] string Repo,
[property: CommandSwitch("--sql-file")] string SqlFile
) : AzOptions
{
    [CommandSwitch("--action-name")]
    public string? ActionName { get; set; }

    [BooleanCommandSwitch("--allow-push")]
    public bool? AllowPush { get; set; }

    [CommandSwitch("--branch")]
    public string? Branch { get; set; }

    [CommandSwitch("--database-name")]
    public string? DatabaseName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--server-name")]
    public string? ServerName { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}