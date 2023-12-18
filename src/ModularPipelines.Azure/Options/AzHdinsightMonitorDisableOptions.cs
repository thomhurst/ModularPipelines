using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight", "monitor", "disable")]
public record AzHdinsightMonitorDisableOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-validation-timeout")]
    public bool? NoValidationTimeout { get; set; }

    [CommandSwitch("--primary-key")]
    public string? PrimaryKey { get; set; }
}

