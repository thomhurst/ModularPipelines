using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "config", "hostname", "list")]
public record AzFunctionappConfigHostnameListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--webapp-name")] string WebappName
) : AzOptions
{
    [CommandSwitch("--slot")]
    public string? Slot { get; set; }
}