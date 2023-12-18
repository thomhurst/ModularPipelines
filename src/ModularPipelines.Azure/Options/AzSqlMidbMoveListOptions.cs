using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "midb", "move", "list")]
public record AzSqlMidbMoveListOptions : AzOptions
{
    [CommandSwitch("--dest-mi")]
    public string? DestMi { get; set; }

    [CommandSwitch("--dest-resource-group")]
    public string? DestResourceGroup { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--latest")]
    public bool? Latest { get; set; }

    [CommandSwitch("--managed-instance")]
    public string? ManagedInstance { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}