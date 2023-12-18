using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight", "ure-monitor", "enable")]
public record AzHdinsightAzureMonitorEnableOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace")] string Workspace
) : AzOptions
{
    [BooleanCommandSwitch("--no-validation-timeout")]
    public bool? NoValidationTimeout { get; set; }

    [CommandSwitch("--primary-key")]
    public string? PrimaryKey { get; set; }
}