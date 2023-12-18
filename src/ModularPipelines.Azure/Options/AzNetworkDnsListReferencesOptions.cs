using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "dns", "list-references")]
public record AzNetworkDnsListReferencesOptions : AzOptions
{
    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }
}