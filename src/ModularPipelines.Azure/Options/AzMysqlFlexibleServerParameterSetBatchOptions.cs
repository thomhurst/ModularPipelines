using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mysql", "flexible-server", "parameter", "set-batch")]
public record AzMysqlFlexibleServerParameterSetBatchOptions(
[property: CommandSwitch("--args")] string Args
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--server-name")]
    public string? ServerName { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}