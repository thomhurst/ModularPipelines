using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mysql", "flexible-server", "deploy", "setup")]
public record AzMysqlFlexibleServerDeploySetupOptions(
[property: CliOption("--admin-password")] string AdminPassword,
[property: CliOption("--admin-user")] string AdminUser,
[property: CliOption("--repo")] string Repo,
[property: CliOption("--sql-file")] string SqlFile
) : AzOptions
{
    [CliOption("--action-name")]
    public string? ActionName { get; set; }

    [CliFlag("--allow-push")]
    public bool? AllowPush { get; set; }

    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--server-name")]
    public string? ServerName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}