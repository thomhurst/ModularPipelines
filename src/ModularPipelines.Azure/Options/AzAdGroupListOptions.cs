using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "group", "list")]
public record AzAdGroupListOptions : AzOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }
}