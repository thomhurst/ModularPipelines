using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedmachine", "extension", "show")]
public record AzConnectedmachineExtensionShowOptions : AzOptions
{
    [CommandSwitch("--extension-name")]
    public string? ExtensionName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--machine-name")]
    public string? MachineName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}