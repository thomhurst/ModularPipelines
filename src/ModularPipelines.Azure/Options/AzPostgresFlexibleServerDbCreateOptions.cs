using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("postgres", "flexible-server", "db", "create")]
public record AzPostgresFlexibleServerDbCreateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server-name")] string ServerName
) : AzOptions
{
    [CliOption("--charset")]
    public string? Charset { get; set; }

    [CliOption("--collation")]
    public string? Collation { get; set; }

    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }
}