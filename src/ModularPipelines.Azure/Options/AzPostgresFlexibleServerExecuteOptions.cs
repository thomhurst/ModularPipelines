using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("postgres", "flexible-server", "execute")]
public record AzPostgresFlexibleServerExecuteOptions(
[property: CliOption("--admin-password")] string AdminPassword,
[property: CliOption("--admin-user")] string AdminUser,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }

    [CliOption("--file-path")]
    public string? FilePath { get; set; }

    [CliOption("--querytext")]
    public string? Querytext { get; set; }
}