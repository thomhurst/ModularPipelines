using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("palo-alto", "cloudngfw", "local-rulestack", "fqdnlist", "create")]
public record AzPaloAltoCloudngfwLocalRulestackFqdnlistCreateOptions(
[property: CliOption("--fqdn-list")] string FqdnList,
[property: CliOption("--local-rulestack-name")] string LocalRulestackName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--audit-comment")]
    public string? AuditComment { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}