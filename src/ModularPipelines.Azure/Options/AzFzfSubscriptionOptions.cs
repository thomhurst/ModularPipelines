using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fzf", "subscription")]
public record AzFzfSubscriptionOptions : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [BooleanCommandSwitch("--no-default")]
    public bool? NoDefault { get; set; }
}