using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedmachine", "extension", "image", "show")]
public record AzConnectedmachineExtensionImageShowOptions : AzOptions
{
    [CommandSwitch("--extension-type")]
    public string? ExtensionType { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--publisher")]
    public string? Publisher { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}