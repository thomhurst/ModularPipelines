using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mysql", "flexible-server", "gtid", "reset")]
public record AzMysqlFlexibleServerGtidResetOptions(
[property: CliOption("--gtid-set")] string GtidSet,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server-name")] string ServerName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}