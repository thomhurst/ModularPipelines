using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("desktopvirtualization", "applicationgroup", "list")]
public record AzDesktopvirtualizationApplicationgroupListOptions : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}