using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns-resolver", "vnet-link", "create")]
public record AzDnsResolverVnetLinkCreateOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--ruleset-name")] string RulesetName
) : AzOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}