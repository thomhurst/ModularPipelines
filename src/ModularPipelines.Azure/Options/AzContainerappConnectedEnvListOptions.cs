using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "connected-env", "list")]
public record AzContainerappConnectedEnvListOptions : AzOptions
{
    [CommandSwitch("--custom-location")]
    public string? CustomLocation { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}