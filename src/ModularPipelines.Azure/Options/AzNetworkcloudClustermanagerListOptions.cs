using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "clustermanager", "list")]
public record AzNetworkcloudClustermanagerListOptions : AzOptions
{
    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}