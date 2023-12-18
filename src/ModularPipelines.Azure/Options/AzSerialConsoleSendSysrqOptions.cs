using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("serial-console", "send", "sysrq")]
public record AzSerialConsoleSendSysrqOptions(
[property: CommandSwitch("--input")] string Input,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }
}