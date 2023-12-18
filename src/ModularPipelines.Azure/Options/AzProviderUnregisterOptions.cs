using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("provider", "unregister")]
public record AzProviderUnregisterOptions(
[property: CommandSwitch("--namespace")] string Namespace
) : AzOptions
{
    [BooleanCommandSwitch("--wait")]
    public bool? Wait { get; set; }
}