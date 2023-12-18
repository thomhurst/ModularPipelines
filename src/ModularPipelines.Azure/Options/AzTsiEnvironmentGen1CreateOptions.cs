using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tsi", "environment", "gen1", "create")]
public record AzTsiEnvironmentGen1CreateOptions(
[property: CommandSwitch("--data-retention-time")] string DataRetentionTime,
[property: CommandSwitch("--environment-name")] string EnvironmentName,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
    [CommandSwitch("--exceeded-behavior")]
    public string? ExceededBehavior { get; set; }

    [CommandSwitch("--key-properties")]
    public string? KeyProperties { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}