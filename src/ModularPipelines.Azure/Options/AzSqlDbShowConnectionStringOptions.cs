using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "db", "show-connection-string")]
public record AzSqlDbShowConnectionStringOptions(
[property: CommandSwitch("--client")] string Client
) : AzOptions
{
    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--server")]
    public string? Server { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}