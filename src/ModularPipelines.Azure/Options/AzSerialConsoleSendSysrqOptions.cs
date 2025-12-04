using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("serial-console", "send", "sysrq")]
public record AzSerialConsoleSendSysrqOptions(
[property: CliOption("--input")] string Input,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--instance-id")]
    public string? InstanceId { get; set; }
}