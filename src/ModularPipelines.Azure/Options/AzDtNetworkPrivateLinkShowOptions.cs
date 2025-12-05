using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dt", "network", "private-link", "show")]
public record AzDtNetworkPrivateLinkShowOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--link-name")] string LinkName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}