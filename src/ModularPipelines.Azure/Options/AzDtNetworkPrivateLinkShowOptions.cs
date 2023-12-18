using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "network", "private-link", "show")]
public record AzDtNetworkPrivateLinkShowOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--link-name")] string LinkName
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}