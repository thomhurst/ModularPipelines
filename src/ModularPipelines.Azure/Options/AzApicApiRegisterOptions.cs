using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apic", "api", "register")]
public record AzApicApiRegisterOptions(
[property: CommandSwitch("--api-location")] string ApiLocation,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [CommandSwitch("--environment-name")]
    public string? EnvironmentName { get; set; }
}