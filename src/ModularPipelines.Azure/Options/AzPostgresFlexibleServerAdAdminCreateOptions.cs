using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("postgres", "flexible-server", "ad-admin", "create")]
public record AzPostgresFlexibleServerAdAdminCreateOptions(
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--object-id")] string ObjectId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server-name")] string ServerName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}