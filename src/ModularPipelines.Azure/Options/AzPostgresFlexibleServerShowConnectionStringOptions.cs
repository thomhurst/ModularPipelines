using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("postgres", "flexible-server", "show-connection-string")]
public record AzPostgresFlexibleServerShowConnectionStringOptions : AzOptions
{
    [CliOption("--admin-password")]
    public string? AdminPassword { get; set; }

    [CliOption("--admin-user")]
    public string? AdminUser { get; set; }

    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--pg-bouncer")]
    public bool? PgBouncer { get; set; }

    [CliOption("--server-name")]
    public string? ServerName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}