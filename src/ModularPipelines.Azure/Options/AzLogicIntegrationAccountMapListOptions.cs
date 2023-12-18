using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logic", "integration-account", "map", "list")]
public record AzLogicIntegrationAccountMapListOptions(
[property: CommandSwitch("--integration-account")] int IntegrationAccount,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}