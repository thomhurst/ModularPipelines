using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "get-access-token")]
public record AzAccountGetAccessTokenOptions : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource")]
    public string? Resource { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--tenant")]
    public string? Tenant { get; set; }
}