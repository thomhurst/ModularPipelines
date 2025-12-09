using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "du", "account", "private-link-resource", "list")]
public record AzIotDuAccountPrivateLinkResourceListOptions(
[property: CliOption("--account")] int Account
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}