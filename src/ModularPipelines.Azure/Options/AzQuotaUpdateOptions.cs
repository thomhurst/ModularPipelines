using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quota", "update")]
public record AzQuotaUpdateOptions(
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--scope")] string Scope
) : AzOptions
{
    [CommandSwitch("--limit-object")]
    public string? LimitObject { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--properties")]
    public string? Properties { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }
}