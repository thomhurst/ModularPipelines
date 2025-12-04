using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datamigration", "sql-service", "regenerate-auth-key")]
public record AzDatamigrationSqlServiceRegenerateAuthKeyOptions : AzOptions
{
    [CliOption("--auth-key1")]
    public string? AuthKey1 { get; set; }

    [CliOption("--auth-key2")]
    public string? AuthKey2 { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--key-name")]
    public string? KeyName { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}