using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "db", "import")]
public record AzSqlDbImportOptions(
[property: CliOption("--admin-password")] string AdminPassword,
[property: CliOption("--admin-user")] string AdminUser,
[property: CliOption("--storage-key")] string StorageKey,
[property: CliOption("--storage-key-type")] string StorageKeyType,
[property: CliOption("--storage-uri")] string StorageUri
) : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--server")]
    public string? Server { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}