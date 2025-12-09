using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "host", "list-resize-options")]
public record AzVmHostListResizeOptionsOptions : AzOptions
{
    [CliOption("--host-group")]
    public string? HostGroup { get; set; }

    [CliOption("--host-name")]
    public string? HostName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}