using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mariadb", "db", "create")]
public record AzMariadbDbCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--server-name")] string ServerName
) : AzOptions
{
    [CommandSwitch("--charset")]
    public string? Charset { get; set; }

    [CommandSwitch("--collation")]
    public string? Collation { get; set; }
}