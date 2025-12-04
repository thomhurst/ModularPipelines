using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "db-arc", "restore")]
public record AzSqlDbArcRestoreOptions(
[property: CliOption("--dest-name")] string DestName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server")] string Server
) : AzOptions
{
    [CliOption("--time")]
    public string? Time { get; set; }
}