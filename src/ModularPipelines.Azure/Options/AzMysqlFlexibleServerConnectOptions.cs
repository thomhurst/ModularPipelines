using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mysql", "flexible-server", "connect")]
public record AzMysqlFlexibleServerConnectOptions(
[property: CliOption("--admin-user")] string AdminUser,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--admin-password")]
    public string? AdminPassword { get; set; }

    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }

    [CliOption("--interactive")]
    public string? Interactive { get; set; }
}