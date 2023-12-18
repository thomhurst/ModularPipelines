using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "midb", "restore")]
public record AzSqlMidbRestoreOptions(
[property: CommandSwitch("--dest-name")] string DestName,
[property: CommandSwitch("--time")] string Time
) : AzOptions
{
    [CommandSwitch("--deleted-time")]
    public string? DeletedTime { get; set; }

    [CommandSwitch("--dest-mi")]
    public string? DestMi { get; set; }

    [CommandSwitch("--dest-resource-group")]
    public string? DestResourceGroup { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--managed-instance")]
    public string? ManagedInstance { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--source-sub")]
    public string? SourceSub { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}