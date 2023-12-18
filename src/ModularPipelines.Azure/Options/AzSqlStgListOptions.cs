using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "stg", "list")]
public record AzSqlStgListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--instance-name")]
    public string? InstanceName { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}