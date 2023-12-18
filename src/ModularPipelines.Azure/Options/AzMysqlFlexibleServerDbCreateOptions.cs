using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mysql", "flexible-server", "db", "create")]
public record AzMysqlFlexibleServerDbCreateOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--server-name")] string ServerName
) : AzOptions
{
    [CommandSwitch("--charset")]
    public string? Charset { get; set; }

    [CommandSwitch("--collation")]
    public string? Collation { get; set; }

    [CommandSwitch("--database-name")]
    public string? DatabaseName { get; set; }
}