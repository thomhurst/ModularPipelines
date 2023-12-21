using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "db", "export")]
public record AzSqlDbExportOptions(
[property: CommandSwitch("--admin-password")] string AdminPassword,
[property: CommandSwitch("--admin-user")] string AdminUser,
[property: CommandSwitch("--storage-key")] string StorageKey,
[property: CommandSwitch("--storage-key-type")] string StorageKeyType,
[property: CommandSwitch("--storage-uri")] string StorageUri
) : AzOptions
{
    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--server")]
    public string? Server { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}